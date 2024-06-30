using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_Model<T> : MonoBehaviour, IModel where T : IPresenter{
        private T _presenter;

        protected virtual void Awake() {
            _presenter = this.GetComponent<T>();
        }
    }
}
