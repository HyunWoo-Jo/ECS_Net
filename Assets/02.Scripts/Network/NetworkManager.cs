using UnityEngine;
using Game.DesignPattern;
using Unity.NetCode;
using Unity.Networking.Transport;
using System;
using Unity.Entities;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
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
        private Action _loadListner = null;
        
        public string Ip { get { return _ip; } }
        public ushort Port { get { return _port; } }

        public NetworkConnectingType NetworkConnectingType {
            get { return _networkConnectingType; }
        }

        public void AddLoadListener(Action action) {
            _loadListner += action;
        }
        public void DeleteLoadListener(Action action) {
            _loadListner -= action;
        }


        public void SetIP(string ip) { _ip = ip; }
        public void SetPort(ushort port) { _port = port; }

        public override void Awake() {
            base.Awake();
        }

        /// <summary>
        /// Client 월드로 셋팅
        /// </summary>
        public void LoadClient() {
            _networkConnectingType = NetworkConnectingType.Client;
            NetworkEndpoint endPoint = NetworkEndpoint.Parse(_ip, _port);
            ClientServerBootstrap.DefaultConnectAddress = endPoint;
            _loadListner?.Invoke();
        }

        /// <summary>
        /// Client Server 월드로 셋팅
        /// </summary>
        public void LoadServerClient() {
            _networkConnectingType = NetworkConnectingType.ServerClient;
            _loadListner?.Invoke();
        }

        public int GetUseAblePort() {
            int startPort = 1024;  // 사용자 포트 범위 시작
            int endPort = 65535;   // 사용자 포트 범위 끝

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] activeTcpListeners = properties.GetActiveTcpListeners();
            int[] usedPorts = activeTcpListeners.Select(p => p.Port).ToArray();

            for (int port = startPort; port <= endPort; port++) {
                if (!usedPorts.Contains(port)) {
                    return port;
                }
            }
            return -1;
        }
 

    }
}
