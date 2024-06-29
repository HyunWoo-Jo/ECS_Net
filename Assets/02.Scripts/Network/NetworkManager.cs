using UnityEngine;
using Game.DesignPattern;
using Unity.Collections;
using System.Net;
using Unity.NetCode;
namespace Game.Network
{
    public enum NetworkConnectingType {
        Client,
        Server,
        ClientServer,
    }
    public class NetworkManager : Singleton<NetworkManager>
    {
        private string _ip;
        private ushort _port;
        private NetworkConnectingType _networkConnectingType;

        public string Ip { get { return _ip; } }
        public ushort Port { get { return _port; } }
        public NetworkConnectingType NetworkConnectingType {
            get { return _networkConnectingType; }
        }

        public void SetIP(string ip) { _ip = ip; }
        public void SetPort(ushort port) { _port = port; }
        public void OnSetConnectingType(NetworkConnectingType connectingType) {
            _networkConnectingType = connectingType;
        }

        public override void Awake() {
            base.Awake();
        }

        public void LoadClient() {

        }

        public void LoadClientServer() {
            ClientServerBootstrap.AutoConnectPort = _port;

        }

    }
}
