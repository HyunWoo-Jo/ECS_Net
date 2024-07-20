using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Collections;
using Unity.NetCode;
using Game.Ecs;

namespace Game.Network
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct NetworkClientSpawnSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<NetworkId>()
                .WithNone<NetworkStreamInGame>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (id, entity) in SystemAPI.Query<NetworkId>().WithEntityAccess().WithNone<NetworkStreamInGame>()) {
                ecb.AddComponent<NetworkStreamInGame>(entity);
                Entity reqEntity = ecb.CreateEntity();
                ecb.AddComponent(reqEntity, new SpawnRpcCommand { spawnType = Data.SpawnType.Player});
                ecb.AddComponent(reqEntity, new SendRpcCommandRequest { TargetConnection = entity});
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }


    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct NetworkServerSpawnSystem : ISystem {
        private ComponentLookup<NetworkId> networkIdFromEntity;
        private SpawnerComponent spawnerCoponent;
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<NetworkId>()
                .WithNone<NetworkStreamInGame>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
            networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);

          
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            networkIdFromEntity.Update(ref state);

            spawnerCoponent = SystemAPI.GetSingleton<SpawnerComponent>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach(var (reqSrc, spawnRpc, reqEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, SpawnRpcCommand>().WithEntityAccess()) {
                ecb.AddComponent<NetworkStreamInGame>(reqSrc.ValueRO.SourceConnection);
                var networkId = networkIdFromEntity[reqSrc.ValueRO.SourceConnection];

                switch (spawnRpc.spawnType) {
                    case Data.SpawnType.Player:
                    Entity playerEntity = ecb.Instantiate(spawnerCoponent.playerEntity);
                    ecb.SetComponent(playerEntity, new GhostOwner { NetworkId = networkId.Value });
                    ecb.AppendToBuffer(reqSrc.ValueRO.SourceConnection, new LinkedEntityGroup { Value = playerEntity });
                    ecb.DestroyEntity(reqEntity);
                    break;
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
