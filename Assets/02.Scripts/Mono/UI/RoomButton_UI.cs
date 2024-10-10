using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using Game.Utills;
using Game.Network;
namespace Game.Mono.UI
{
    public class RoomButton_UI : MonoBehaviour, ILoadAble   
    {
        private RoomData _roomData;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private GameObject _isPublicText;
        private Action<string> _buttonClickListener;

        [Header("Button")]
        [SerializeField] private EventTrigger _joinButton;
        private void Awake() {
            _joinButton.AddDownButton(OnButtonDown);
        }
        public void UpdateRoomData(RoomData roomData) {
            _roomData = roomData;
            UpdateUI();
        }

        private void OnButtonDown() {
            if (_roomData.isPublic) { // 공개방
                JoinServerManager.Instance.JoinRoom(_roomData.roomHash); // 연결

            } else { // 비공개방
                Password_UI_Popup popup = UI_Manager.Instance.InstancePopupUI<Password_UI_Popup>();
                popup.SetRoomData(_roomData);
            }
        }

        private void UpdateUI() {
            _userNameText.text = _roomData.userName;
            _roomNameText.text = _roomData.roomName;
            if (_roomData.isPublic) {
                _isPublicText.SetActive(false);
            } else {
                _isPublicText.SetActive(true);
            }
        }

    }
}
