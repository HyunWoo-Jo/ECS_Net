using UnityEngine;

namespace Game.Mono.UI
{
    public abstract class Popup : MonoBehaviour
    {

        /// <summary>
        /// UI �����Ǿ����� �۵�
        /// </summary>
        internal abstract void OnCreate();

        /// <summary>
        /// UI ����
        /// </summary>
        internal virtual void OnClose() {
            Popup_UI_Manager.Instance.CloseUI(this);
            Destroy(this.gameObject);
        }
    }
}
