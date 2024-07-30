using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs
{
    public class MovementMono : MonoBehaviour
    {
        [SerializeField] private float _acceleration;
        [SerializeField] private float _maxVelocity;
        [SerializeField] private float _rotationSpeed;

        private class MovementBaker : Baker<MovementMono> {
            public override void Bake(MovementMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);

                AddComponent(entity, new MovementProperties { 
                    acceleration = authoring._acceleration,
                    maxVelocity = new float3(authoring._maxVelocity, float.MaxValue, authoring._maxVelocity), 
                    rotationSpeed = authoring._rotationSpeed 
                }); 
            }
        }
    }
}
