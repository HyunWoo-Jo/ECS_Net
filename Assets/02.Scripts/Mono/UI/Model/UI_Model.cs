using UnityEngine;

namespace Game.Mono.UI
{
    internal class UI_Model<T> : MonoBehaviour {
        private T _presenter;

        protected virtual void Awake() {
            _presenter = this.GetComponent<T>();
        }
    }
}
