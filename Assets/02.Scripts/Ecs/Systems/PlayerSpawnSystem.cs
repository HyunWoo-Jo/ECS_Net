using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using UnityEngine.Scripting;
namespace Game.Ecs
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct PlayerSpawnSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state) { }
        [BurstCompile]
        void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        void OnUpdate(ref SystemState state) { 
            
        
        }
    }
}
