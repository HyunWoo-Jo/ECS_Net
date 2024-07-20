using Game.Data;
using Unity.NetCode;
using UnityEngine;

public struct SpawnRpcCommand : IRpcCommand {
    public SpawnType spawnType;
}

