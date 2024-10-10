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
        /// <summary>
        /// Err Popup 출력
        /// </summary>
        /// <param name="str"></param>
        protected virtual void ShowErrUI(string str) {
            UI_Manager.Instance.ShowErrPopup(str);
        }
    }
}
