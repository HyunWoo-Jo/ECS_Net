using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public partial struct SpawnerComponent : IComponentData {
        public Entity playerEntity;
    }
}
