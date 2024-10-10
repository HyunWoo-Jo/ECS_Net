using UnityEngine;
using Game.Mono;
using UnityEngine.EventSystems;
using Game.Utills;
using Game.Network;
namespace Game.Mono.UI
{
    public class RoomList_UI_Popup : Popup
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private EventTrigger _backButton;
        internal override void OnOpen() {
            _backButton.AddDownButton(OnCloseButton);
        }

        internal override void OnClose() {
            base.OnClose();
        }

        internal Transform GetContentTr() {
            return _content.transform;
        }
        internal void SetContentHeight(float height) {
            Vector2 size = _content.sizeDelta;
            size.y = height;
            _content.sizeDelta = size;
        }


        /// <summary>
        /// Back button
        /// </summary>
        private void OnCloseButton() {
            OnClose();
        }



       
    }
}
