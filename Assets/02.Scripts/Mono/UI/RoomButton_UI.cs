using UnityEngine;
using System;
using TMPro;
namespace Game.Mono.UI
{
    public class RoomButton_UI : MonoBehaviour
    {
        private RoomData _roomData;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private GameObject _isPublic;
        private Action<string> _buttonClickListener; 

        public void AddButtonClickListener(Action<string> action) {
            _buttonClickListener += action;
        }

        public void UpdateRoomData(RoomData roomData) {
            _roomData = roomData;
            UpdateUI();
        }

        public void OnButtonDown() {
            _buttonClickListener?.Invoke(_roomData.roomHash); // 클릭시 hash를 전달
        }

        private void UpdateUI() {
            _userNameText.text = _roomData.userName;
            _roomNameText.text = _roomData.roomName;
            if (_roomData.isPublic) {
                _isPublic.SetActive(false);
            } else {
                _isPublic.SetActive(true);
            }
        }

    }
}
