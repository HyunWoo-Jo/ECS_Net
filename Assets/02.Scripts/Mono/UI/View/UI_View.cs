using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_View<T> : MonoBehaviour
    {
        private T _ui_presenter;

        protected virtual void Awake() {
            _ui_presenter = this.GetComponent<T>();
        }
    }
}
