using UnityEngine;

namespace Game.Mono.UI
{
    public class UI_Model<T> : MonoBehaviour, IModel where T : IPresenter{
        protected T _presenter;

        protected virtual void Awake() {
            _presenter = this.GetComponent<T>();
        }
    }
}
