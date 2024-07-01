using UnityEngine;
using Game.DesignPattern;
using Unity.NetCode;
using Unity.Networking.Transport;
using System;
namespace Game.Network
{
    public enum NetworkConnectingType {
        Client,
        Server,
        ServerClient,
    }
    public class NetworkManager : Singleton<NetworkManager>
    {
        [SerializeField] private string _ip;
        [SerializeField] private ushort _port;
        [SerializeField] private NetworkConnectingType _networkConnectingType;
        public Action loadListner = null;
        
        public string Ip { get { return _ip; } }
        public ushort Port { get { return _port; } }

        public NetworkConnectingType NetworkConnectingType {
            get { return _networkConnectingType; }
        }

        public void SetIP(string ip) { _ip = ip; }
        public void SetPort(ushort port) { _port = port; }

        public override void Awake() {
            base.Awake();
        }

        public void LoadClient() {
            _networkConnectingType = NetworkConnectingType.Client;
            NetworkEndpoint endPoint = NetworkEndpoint.Parse(_ip, _port);
            ClientServerBootstrap.DefaultConnectAddress = endPoint;
            ClientServerBootstrap.AutoConnectPort = _port;
            loadListner?.Invoke();
        }

        public void LoadServerClient() {
            _networkConnectingType = NetworkConnectingType.ServerClient;
            ClientServerBootstrap.AutoConnectPort = _port;
            loadListner?.Invoke();
        }

    }
}
