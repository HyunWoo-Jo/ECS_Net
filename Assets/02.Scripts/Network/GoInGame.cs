using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;
using UnityEngine;

namespace Game.Network {
    /// <summary>
    /// This allows sending RPCs between a stand alone build and the editor for testing purposes in the event when you finish this example
    /// you want to connect a server-client stand alone build to a client configured editor instance. 
    /// </summary>
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ThinClientSimulation)] 
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [CreateAfter(typeof(RpcSystem))]
    public partial struct SetRpcSystemDynamicAssemblyListSystem : ISystem {
        public void OnCreate(ref SystemState state) {
            SystemAPI.GetSingletonRW<RpcCollection>().ValueRW.DynamicAssemblyList = true;
            state.Enabled = false;
        }
    }

    // RPC request from client to server for game to go "in game" and send snapshots / inputs
    public struct GoInGameRequest : IRpcCommand {
    }

    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct GoInGameClientSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            // Run only on entities with a CubeSpawner component data 
            state.RequireForUpdate<CubeSpawner>();

            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<NetworkId>()
                .WithNone<NetworkStreamInGame>(); // NetworkId�� Stream ������Ʈ �˻� 

            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (id, entity) in SystemAPI.Query<RefRO<NetworkId>>().WithEntityAccess().WithNone<NetworkStreamInGame>()) { // networkid�� Entity�� �˻� NetworkStream�� �������
                commandBuffer.AddComponent<NetworkStreamInGame>(entity); // NetworkStream�� �߰�
                var req = commandBuffer.CreateEntity(); // ���ο� Entity ����
                commandBuffer.AddComponent<GoInGameRequest>(req); // rpc ��� ������Ʈ �߰�
                commandBuffer.AddComponent(req, new SendRpcCommandRequest { TargetConnection = entity }); // send RpcCommand�� ���ο� Entity�� �߰� �ϸ鼭 ����� networkId ������Ʈ�� �߰�
                Debug.Log("?");
            }
            commandBuffer.Playback(state.EntityManager);
        }
    }

    [BurstCompile]
    // When server receives go in game request, go in game and delete request
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GoInGameServerSystem : ISystem {
        private ComponentLookup<NetworkId> networkIdFromEntity;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<CubeSpawner>();

            // query GoinR, RcRpc
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<GoInGameRequest>()
                .WithAll<ReceiveRpcCommandRequest>();
            state.RequireForUpdate(state.GetEntityQuery(builder)); // Ư�� ������Ʈ�� �������� ������Ʈ
            networkIdFromEntity = state.GetComponentLookup<NetworkId>(true); // lookup
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            // Get the prefab to instantiate
            var prefab = SystemAPI.GetSingleton<CubeSpawner>().Cube;

            // Ge the name of the prefab being instantiated
            state.EntityManager.GetName(prefab, out var prefabName);
            var worldName = new FixedString32Bytes(state.WorldUnmanaged.Name);

            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            networkIdFromEntity.Update(ref state); // lockup ����

            foreach (var (reqSrc, reqEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequest>().WithEntityAccess()) { // GoinGrameRequest�� ������ ReceiveRpcCommandRequest, Entity ��� �˻� 
                commandBuffer.AddComponent<NetworkStreamInGame>(reqSrc.ValueRO.SourceConnection); // NetworkStream�� ReceiveRpc Entity�� �߰�
                // Get the NetworkId for the requesting client
                var networkId = networkIdFromEntity[reqSrc.ValueRO.SourceConnection]; // lookup���� NetworkId�� �˻�

                // Log information about the connection request that includes the client's assigned NetworkId and the name of the prefab spawned.
                UnityEngine.Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game, spawning a Ghost '{prefabName}' for them!");

                // Instantiate the prefab
                var player = commandBuffer.Instantiate(prefab); // cube ����
                // Associate the instantiated prefab with the connected client's assigned NetworkId
                commandBuffer.SetComponent(player, new GhostOwner { NetworkId = networkId.Value }); // Ghost ID�� Network ID�� ����

                // Add the player to the linked entity group so it is destroyed automatically on disconnect
                commandBuffer.AppendToBuffer(reqSrc.ValueRO.SourceConnection, new LinkedEntityGroup { Value = player }); //LInkedEntityGroup ���ۿ� Cube�� �߰�
                commandBuffer.DestroyEntity(reqEntity); // ���� ReceiveRpcCommandRequest ������Ʈ ����
            }
            commandBuffer.Playback(state.EntityManager);
        }
    }
}
