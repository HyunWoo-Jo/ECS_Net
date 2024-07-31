using UnityEngine;
using Unity.NetCode;
using Unity.Burst;


namespace Game.Network
{

    

    /// <summary>
    /// server client Boot
    /// </summary>
    /// 
    [BurstCompile]

    public class Bootstrap : ClientServerBootstrap {
 
        public override bool Initialize(string defaultWorldName) {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            bool isMainScene = sceneName == "MainScene";
            if (isMainScene) {
                if (NetworkManager.Instance != null) {
                    AutoConnectPort = NetworkManager.Instance.Port;
                } else {
                    AutoConnectPort = 7979;
                }
                CreateDefaultClientServerWorlds();
            } else {       
                CreateLocalWorld(defaultWorldName);
            }
            return true;
        }
    }
}
