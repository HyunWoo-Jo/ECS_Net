using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    [RequireComponent(typeof(MovementMono))]
    [DisallowMultipleComponent]
    public class InputMono : MonoBehaviour
    {
        private class InputBaker : Baker<InputMono> {
            public override void Bake(InputMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new NetMoveInputProperties { });

            }
        }
    }
}
