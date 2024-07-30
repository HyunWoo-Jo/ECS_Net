using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.NetCode;
namespace Game.Ecs
{

    /// <summary>
    /// Entity의 이동을 담당하는 시스템
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MovementProperties>();
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {        
            float deltaTime = SystemAPI.Time.DeltaTime;
            new MoveJob { deltaTime = deltaTime }.ScheduleParallel();
        
        }
        [BurstCompile]
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
    }
}
