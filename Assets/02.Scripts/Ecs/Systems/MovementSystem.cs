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
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
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
            var moveJob = new MoveCubeJob {
                deltaTime = SystemAPI.Time.DeltaTime
            };
            state.Dependency = moveJob.ScheduleParallel(state.Dependency);
        }
        [BurstCompile]
        [WithAll(typeof(Simulate))]
        partial struct MoveCubeJob : IJobEntity {
            public float deltaTime;


            public void Execute(NetMoveInputProperties playerInput, MovementProperties moveProperties,  RefRW<LocalTransform> transformRefRW) {
                var moveInput = new float3(playerInput.horizontal, 0, playerInput.vertical);
                transformRefRW.ValueRW.Position += math.normalizesafe(moveInput) * deltaTime * moveProperties.moveSpeed;
            }

        }
    }
}
