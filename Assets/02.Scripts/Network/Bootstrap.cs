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
            return base.Initialize(defaultWorldName);
        }
    }
}
