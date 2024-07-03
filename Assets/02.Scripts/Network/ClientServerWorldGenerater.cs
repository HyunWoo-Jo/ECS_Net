using UnityEngine;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;
namespace Game.Network
{

    // NetworkManager 셋팅에 따라 클라이언트 월드와 서버 월드를 생성
    public class ClientServerWorldGenerater : MonoBehaviour
    {
        private ushort _port;

        private void Awake()
        {
            _port = NetworkManager.Instance.Port;
            if(NetworkManager.Instance.NetworkConnectingType == NetworkConnectingType.ServerClient) {
                ServerWorldCreate();
            }
        }
        // 서버와 클라리언트 월드를 생성
        private void ServerWorldCreate() {
            World server = ClientServerBootstrap.CreateServerWorld("ServerWorld");
            World client = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            DestroyLocalWorld();
            World.DefaultGameObjectInjectionWorld ??= server;

            // server
            {
                EntityQuery drvQuery = server.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                NetworkEndpoint enp = ClientServerBootstrap.DefaultListenAddress.WithPort(_port);
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(enp);

                // Lagacy
                //Entity listenRequestEntity = server.EntityManager.CreateEntity(typeof(NetworkStreamRequestListen));
                //server.EntityManager.SetComponentData(listenRequestEntity, new NetworkStreamRequestListen { Endpoint = enp });
            }
            // client
            {
                EntityQuery drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                NetworkEndpoint enp = ClientServerBootstrap.DefaultConnectAddress.WithPort(_port);
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, enp);

                // Lagacy
                //Entity listenRequestEntity = client.EntityManager.CreateEntity(typeof(NetworkStreamRequestConnect));
                //client.EntityManager.SetComponentData(listenRequestEntity, new NetworkStreamRequestConnect { Endpoint = enp });
            }
        }

        private void ClientWorldCreate() { 



        }

        /// <summary>
        /// 기존 local World 제거
        /// </summary>
        private void DestroyLocalWorld() {
            foreach (World world in World.All) {
                if (world.Flags == WorldFlags.Game) {
                    world.Dispose();
                    break;
                }
            }
        }
    }
}
