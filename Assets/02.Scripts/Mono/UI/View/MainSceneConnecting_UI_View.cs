using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.ObjectModel;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [Header("InputField")]
        [SerializeField] private TMP_InputField _userInputField;

        [Header("Main Button")]
        [SerializeField] private GameObject _createRoomButton;
        [SerializeField] private GameObject _roomListButton;
        
        [Header("Room")]
        [SerializeField] private RoomButton_UI _roomUIButton;
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private GameObject _roomContent;
        [SerializeField] private EventTrigger _room_backButton;

        [Header("Password Panel")]
        [SerializeField] private GameObject _passwordPanel;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private EventTrigger _password_backButton;
        [SerializeField] private EventTrigger _password_enterButton;

        [Header("Err Panel")]
        [SerializeField] private GameObject _errPanel;
        [SerializeField] private TextMeshProUGUI _errText;
        [SerializeField] private EventTrigger _err_enterButton;

        [Header("Create Room Panel")]
        [SerializeField] private GameObject _createRoomPanel;
        [SerializeField] private EventTrigger _createRoom_backButton;
        [SerializeField] private EventTrigger _createRoom_createButton;

        internal void UpdateRoomUI() {
            ReadOnlyCollection<RoomData> roomDataList = _presenter.GetRoomList_RO();
            foreach (RoomData roomData in roomDataList) {

            }
        }
       
        internal void ShowErrText(string msg) {
            _errPanel.SetActive(true);
            _errText.text = msg;
        } 

        #region Button
        private void ButtonInit() {

            
        }
      


        // 방 생성 (버튼에 항상하여 사용)
        private void OnCreateRoom() {
            _presenter.CreateRoom();
        }
        // roomList를 받아와 roomList ui 생성 (버튼에 항상하여 사용)
        private void OnCreateRoomList() {
            _presenter.CreateRoomList();
        }
        // room panel 끄기
        private void OnCloseRoomPanel() {
            _roomPanel.SetActive(false);
            _createRoomButton.SetActive(true);
            _roomListButton.SetActive(true);
        }
     

        // pasword Input panel 끄기
        private void OnClosePasswordPanel() {
            _passwordPanel.gameObject.SetActive(false);
        }
        // Password 전송
        private void OnEnterPassword() {

        }
        // err Panel 끄기
        private void OnCloseErrPanel() {
            _errPanel.gameObject.SetActive(false);
        }
        #endregion
    }
}
