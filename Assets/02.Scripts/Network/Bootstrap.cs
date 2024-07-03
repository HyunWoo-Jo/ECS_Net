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
            if (AutoConnectPort != 0) {
                return base.Initialize(defaultWorldName);
            } else {       
                CreateLocalWorld(defaultWorldName);
                return true;
            }
        }
    }
}
