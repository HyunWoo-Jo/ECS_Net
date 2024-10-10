using UnityEngine;

namespace Game.Mono.UI
{
    public abstract class Popup : MonoBehaviour, ILoadAble {

        /// <summary>
        /// UI �����Ǿ����� �۵�
        /// </summary>
        internal abstract void OnOpen();

        /// <summary>
        /// UI ����
        /// </summary>
        internal virtual void OnClose() {
            UI_Manager.Instance.CloseUI(this);
        }
        /// <summary>
        /// Err Popup ���
        /// </summary>
        /// <param name="str"></param>
        protected virtual void ShowErrUI(string str) {
            UI_Manager.Instance.ShowErrPopup(str);
        }
    }
}
