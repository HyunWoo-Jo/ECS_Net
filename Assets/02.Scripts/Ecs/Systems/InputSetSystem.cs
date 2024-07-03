using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
namespace Game.Ecs
{
    /// <summary>
    /// Input 데이타 정보를 사용할 수 있도록 할당해주는 시스템
    /// </summary>
    [UpdateAfter(typeof(MovementSystem))]
    [BurstCompile]
    public partial struct InputSetSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MoveInputComponent>();
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) { 
            
        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            new InputSetJob { }.ScheduleParallel();
        }


        [BurstCompile]
        private partial struct InputSetJob : IJobEntity {
            [BurstCompile]
            private void Execute(RefRO<MoveInputComponent> moveInputRefRO, RefRW<MovementComponent> moveRefRW, Simulate simulate) {
                moveRefRW.ValueRW.moveDirction = new float3(moveInputRefRO.ValueRO.horizontal, 0, moveInputRefRO.ValueRO.vertical);
            }
        }
    }
}
