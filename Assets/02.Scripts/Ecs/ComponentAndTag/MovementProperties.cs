using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs
{
    public partial struct MovementProperties : IComponentData
    {
        public bool isStop;
        public float rotationSpeed;
        public float acceleration;
        public float3 maxVelocity;
    }
}
