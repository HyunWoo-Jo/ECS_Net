using Unity.Entities;
using UnityEngine;
using Unity.NetCode;
namespace Game.Ecs
{
    // Net Command Component
    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
    public partial struct NetMoveInputProperties : IInputComponentData {
        public float horizontal;
        public float vertical;
    }
}
