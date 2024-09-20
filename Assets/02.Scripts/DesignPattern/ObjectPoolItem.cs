using UnityEngine;

namespace Game.DesignPattern
{
    public class ObjectPoolItem : MonoBehaviour
    {
        private ObjectPool _owner;

        internal void Init(ObjectPool owner) {
            _owner = owner;
        }

        public void Refund() {
            _owner.RefundItem(this.gameObject);
        }
    }
}
