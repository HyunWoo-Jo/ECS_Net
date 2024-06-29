using Unity.Entities;
using Unity.NetCode;
using UnityEngine;


namespace Game.Network
{
    public struct Cube : IComponentData {
    }

    public struct CubeInput : IInputComponentData {
        public int Horizontal;
        public int Vertical;
    }

    [DisallowMultipleComponent]

    public class CubeA : MonoBehaviour
    {
        class Bakerq : Baker<CubeA> {
            public override void Bake(CubeA authoring) {
               
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new Cube { });
                AddComponent(entity, new CubeInput { });
            }
        }

    }
  
    // Command
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct SampleCubeInput : ISystem {
        public void OnUpdate(ref SystemState state) {
            bool left = UnityEngine.Input.GetKey("left");
            bool right = UnityEngine.Input.GetKey("right");
            bool down = UnityEngine.Input.GetKey("down");
            bool up = UnityEngine.Input.GetKey("up");

            foreach (var playerInput in SystemAPI.Query<RefRW<CubeInput>>().WithAll<GhostOwnerIsLocal>()) {
                playerInput.ValueRW = default;
                if (left)
                    playerInput.ValueRW.Horizontal -= 1;
                if (right)
                    playerInput.ValueRW.Horizontal += 1;
                if (down)
                    playerInput.ValueRW.Vertical -= 1;
                if (up)
                    playerInput.ValueRW.Vertical += 1;
            }
        }
    }
}
