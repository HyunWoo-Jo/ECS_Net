using UnityEngine;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;
namespace Game.Network
{

    // NetworkManager ���ÿ� ���� Ŭ���̾�Ʈ ����� ���� ���带 ����
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
        // ������ Ŭ�󸮾�Ʈ ���带 ����
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
        /// ���� local World ����
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
