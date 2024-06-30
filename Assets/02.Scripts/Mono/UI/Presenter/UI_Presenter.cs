using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_Presenter<T,U> : MonoBehaviour, IPresenter where T : IView where U : IModel
    {
        private T _view;
        private U _model;

        protected virtual void Awake() {
            _view = this.GetComponent<T>();
            _model = this.GetComponent<U>();
        }
    }
}
