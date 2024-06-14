using System;
using Unity.Entities;
using Unity.NetCode;

namespace Game.Network
{
    [UnityEngine.Scripting.Preserve]
    public class NetworkAutoLoad : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName) {
            AutoConnectPort = 7979;
            return base.Initialize(defaultWorldName);
        }
    }
}
