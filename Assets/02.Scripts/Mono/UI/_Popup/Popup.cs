using UnityEngine;

namespace Game.Mono.UI
{
    public abstract class Popup : MonoBehaviour, ILoadAble {

        /// <summary>
        /// UI 생성되었을시 작동
        /// </summary>
        internal abstract void OnOpen();

        /// <summary>
        /// UI 제거
        /// </summary>
        internal virtual void OnClose() {
            UI_Manager.Instance.CloseUI(this);
        }
    }
}
