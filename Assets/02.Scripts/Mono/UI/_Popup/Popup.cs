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
        internal abstract void OnClose();
    }
}
