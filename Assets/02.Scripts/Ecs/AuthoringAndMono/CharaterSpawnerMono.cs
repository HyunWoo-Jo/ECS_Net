using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public class CharaterSpawnerMono : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObj;

        private class ChracterSpawnerBaker : Baker<CharaterSpawnerMono> {
            public override void Bake(CharaterSpawnerMono authoring) {
                Entity playerEntity = GetEntity(authoring._playerObj, TransformUsageFlags.Dynamic); 
            }
        }

    }
}
