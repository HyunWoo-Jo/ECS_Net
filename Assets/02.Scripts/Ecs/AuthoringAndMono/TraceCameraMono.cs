using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public class TraceCameraMono : MonoBehaviour
    {
        private class TraceCameraBaker : Baker<TraceCameraMono> {
            public override void Bake(TraceCameraMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent<TraceCameraTag>(entity);
            }
        }

    }
}
