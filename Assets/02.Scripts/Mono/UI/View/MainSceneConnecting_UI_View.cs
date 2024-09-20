using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.ObjectModel;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [Header("Main Button")]
        [SerializeField] private GameObject _createRoomButton;
        [SerializeField] private GameObject _roomListButton;
        
        [Header("Room")]
        [SerializeField] private RoomButton_UI _roomUIButton;
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private GameObject _roomContent;

        [Header("Password Panel")]
        [SerializeField] private GameObject _passwordPanel;
        [SerializeField] private TMP_InputField _passwordInputField;

        [Header("Err Panel")]
        [SerializeField] private GameObject _errPanel;
        [SerializeField] private TextMeshProUGUI _errText;


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
        // 방 생성 (버튼에 항상하여 사용)
        public void OnCreateRoom() {
            _presenter.CreateRoom();
        }
        // roomList를 받아와 roomList ui 생성 (버튼에 항상하여 사용)
        public void OnCreateRoomList() {
            _presenter.CreateRoomList();
        }
        // room panel 끄기
        public void OnCloseRoomPanel() {
            _roomPanel.SetActive(false);
            _createRoomButton.SetActive(true);
            _roomListButton.SetActive(true);
        }
     

        // pasword Input panel 끄기
        public void OnClosePasswordPanel() {
            _passwordPanel.gameObject.SetActive(false);
        }
        // Password 전송
        public void OnEnterPassword() {

        }
        // err Panel 끄기
        public void OnCloseErrPanel() {
            _errPanel.gameObject.SetActive(false);
        }
        #endregion
    }
}
