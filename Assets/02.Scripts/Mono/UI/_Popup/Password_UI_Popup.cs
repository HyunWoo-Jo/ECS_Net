using Game.Utills;
using UnityEngine;
using UnityEngine.EventSystems;
using Game.Network;
using TMPro;
namespace Game.Mono.UI
{
    public class Password_UI_Popup : Popup
    {
        private RoomData _roomData;

        [Header("Settings")]
        [SerializeField] private EventTrigger _enterButton;
        [SerializeField] private EventTrigger _cancelButton;
        [SerializeField] private TMP_InputField _passwordText;
        
        internal void SetRoomData(RoomData roomData) {
            _roomData = roomData;
        }
        internal override void OnOpen() { // Init
            _enterButton.AddDownButton(OnEnterButton);
            _cancelButton.AddDownButton(OnClose);
        }
        internal override void OnClose() {
            base.OnClose();
        }

        private void OnEnterButton() { // password 연결 시도
            JoinServerManager.Instance.JoinRoom(_roomData.roomHash, _passwordText.text);
            OnClose();
        }
        private void OnCancel() {
            OnClose();
        }

    }
}
