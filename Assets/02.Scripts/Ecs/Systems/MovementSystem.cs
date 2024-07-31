using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.NetCode;
using Unity.Collections;
namespace Game.Ecs
{

    /// <summary>
    /// Entity의 이동을 담당하는 시스템
    /// </summary>
    [BurstCompile]
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            var builder = new EntityQueryBuilder(Allocator.Temp).
                WithAll<Simulate>().
                WithAll<MovementProperties>().
                WithAll<LocalTransform>();
            var query = state.GetEntityQuery(builder);
            state.RequireForUpdate(query);
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {        
            float deltaTime = SystemAPI.Time.DeltaTime;
            //var moveJob = new MoveJob { deltaTime = deltaTime };
            //state.Dependency = moveJob.ScheduleParallel(state.Dependency);
            var moveJob = new MoveCubeJob {
                tick = SystemAPI.GetSingleton<NetworkTime>().ServerTick,
                fixedCubeSpeed = SystemAPI.Time.DeltaTime * 4
            };
            state.Dependency = moveJob.ScheduleParallel(state.Dependency);
        }
        [BurstCompile]
        [WithAll(typeof(Simulate))]
        private partial struct MoveJob : IJobEntity {
            public float deltaTime;
            [BurstCompile]
            private void Execute(RefRO<MovementProperties> moveRefRO, RefRW<PhysicsVelocity> velocityRefRW) {
                if (moveRefRO.ValueRO.isStop) return;
                float3 direciton = moveRefRO.ValueRO.moveDirction;
                float3 accel = math.normalizesafe(direciton, float3.zero) * moveRefRO.ValueRO.acceleration * deltaTime;
                float3 liner = velocityRefRW.ValueRO.Linear;
                float3 maxVelocity = moveRefRO.ValueRO.maxVelocity;
                liner = math.clamp(liner + accel, -maxVelocity, maxVelocity);
                if (direciton.x == 0) liner.x = 0;
                if (direciton.z == 0) liner.z = 0;
                velocityRefRW.ValueRW.Linear = liner;
            }
        }

        [BurstCompile]
        [WithAll(typeof(Simulate))]
        partial struct MoveCubeJob : IJobEntity {
            public NetworkTick tick;
            public float fixedCubeSpeed;


            public void Execute(MovementProperties playerInput, ref LocalTransform trans) {
                var moveInput = new float2(playerInput.moveDirction.x, playerInput.moveDirction.z);
                moveInput = math.normalizesafe(moveInput) * fixedCubeSpeed;
                trans.Position += new float3(moveInput.x, 0, moveInput.y);
            }

        }
    }
}
