using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public class MovementMono : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        private class MovementBaker : Baker<MovementMono> {
            public override void Bake(MovementMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);

                AddComponent(entity, new MovementComponent { 
                    moveSpeed = authoring._moveSpeed, 
                    rotationSpeed = authoring._rotationSpeed 
                }); 
            }
        }
    }
}
