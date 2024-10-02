using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_Presenter<T,U> : MonoBehaviour, IPresenter where T : IView where U : IModel
    {
        protected T _view;
        protected U _model;

        protected virtual void Awake() {
            _view = this.GetComponent<T>();
            _model = this.GetComponent<U>();
        }

        /// <summary>
        /// Err Popup Ãâ·Â
        /// </summary>
        /// <param name="str"></param>
        protected virtual void ShowErrUI(string str) {
            Err_UI_Popup popup = UI_Manager.Instance.InstancePopupUI<Err_UI_Popup>();
            popup.UpdateText(str);
        }
    }
}
