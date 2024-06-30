using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_View<T> : MonoBehaviour, IView where T : IPresenter
    {
        private T _ui_presenter;

        protected virtual void Awake() {
            _ui_presenter = this.GetComponent<T>();
        }
    }
}
