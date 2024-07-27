using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
namespace Game.Ecs
{
    /// <summary>
    /// Input ����Ÿ ������ ����� �� �ֵ��� �Ҵ����ִ� �ý���
    /// </summary>
    [UpdateAfter(typeof(MovementSystem))]
    [BurstCompile]
    public partial struct InputSetSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<NetMoveInputProperties>();
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
            private void Execute(RefRO<NetMoveInputProperties> moveInputRefRO, RefRW<MovementProperties> moveRefRW, Simulate simulate) {
                moveRefRW.ValueRW.moveDirction = new float3(moveInputRefRO.ValueRO.horizontal, 0, moveInputRefRO.ValueRO.vertical);
            }
        }
    }
}
