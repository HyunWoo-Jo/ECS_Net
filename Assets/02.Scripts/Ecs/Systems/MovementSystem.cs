using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
namespace Game.Ecs
{

    /// <summary>
    /// Entity의 이동을 담당하는 시스템
    /// </summary>
    [BurstCompile]
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MovementComponent>();
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
            private void Execute(RefRO<MovementComponent> moveRefRO, RefRW<LocalTransform> localTrRefRW) {
                if (moveRefRO.ValueRO.isStop) return;
                localTrRefRW.ValueRW.Position += moveRefRO.ValueRO.moveDirction * moveRefRO.ValueRO.moveSpeed * deltaTime;
            }
        }
    }
}
