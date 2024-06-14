using Unity.Entities;
using UnityEngine;

namespace Game.Network {

    public struct CubeSpawner : IComponentData {
        public Entity Cube;
    }

    [DisallowMultipleComponent]
    public class CubeSpawnerAuthoring : MonoBehaviour {
        public GameObject Cube;

        class Baker : Baker<CubeSpawnerAuthoring> {
            public override void Bake(CubeSpawnerAuthoring authoring) {
               
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
                var cubeCoponent = new CubeSpawner { Cube = GetEntity(authoring.Cube, TransformUsageFlags.Dynamic) };
                AddComponent(entity, cubeCoponent);
            }
        }
    }

}
