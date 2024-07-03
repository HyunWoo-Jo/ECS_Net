using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs
{
    public partial struct MovementComponent : IComponentData
    {
        public bool isStop;
        public float moveSpeed;
        public float rotationSpeed;
        public float3 moveDirction;
    }
}
