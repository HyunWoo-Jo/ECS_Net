using Unity.Entities;
using UnityEngine;
using Unity.NetCode;
namespace Game.Ecs
{
    // Net Command Component
    public partial struct NetMoveInputProperties : IInputComponentData {
        public float horizontal;
        public float vertical;
    }
}
