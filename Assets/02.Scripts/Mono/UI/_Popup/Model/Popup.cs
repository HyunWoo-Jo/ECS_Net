using UnityEngine;

namespace Game.Mono.UI
{
    public abstract class Popup : MonoBehaviour
    {

        /// <summary>
        /// UI 생성되었을시 작동
        /// </summary>
        internal abstract void OnCreate();

        /// <summary>
        /// UI 제거
        /// </summary>
        internal virtual void OnClose() {
            Popup_UI_Manager.Instance.CloseUI(this);
            Destroy(this.gameObject);
        }
    }
}
