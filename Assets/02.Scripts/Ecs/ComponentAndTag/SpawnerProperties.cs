using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public partial struct SpawnerProperties : IComponentData {
        public Entity playerEntity;
    }
    
}
