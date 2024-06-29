using UnityEngine;

namespace Game.Mono.UI
{
    internal class UI_Presenter<T,U> : MonoBehaviour
    {
        private T _view;
        private U _model;

        protected virtual void Awake() {
            _view = this.GetComponent<T>();
            _model = this.GetComponent<U>();
        }
    }
}
