using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;

namespace Game.Network
{

    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [CreateAfter(typeof(RpcSystem))]
    public partial struct NetworkInitilizeSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            SetRpcSystemDynamicAssemblyListSystem(ref state);
        }
        [BurstCompile]
        private void SetRpcSystemDynamicAssemblyListSystem(ref SystemState state) {
            SystemAPI.GetSingletonRW<RpcCollection>().ValueRW.DynamicAssemblyList = true;
            state.Enabled = false;
        }
    }

}
