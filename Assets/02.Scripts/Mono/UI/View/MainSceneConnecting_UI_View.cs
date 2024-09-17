using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.ObjectModel;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [SerializeField] private EventTrigger _createRoomTrigger;
        [SerializeField] private EventTrigger _roomListTrigger;
        [SerializeField] private GameObject _roomUI;
        // Init
        private void Start() {
            // create Room; / 버튼에 기능 할당
            EventTrigger.Entry ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnCreateRoomList(); 
            });
            _createRoomTrigger.triggers.Add(ent);

            // room List / 버튼에 기능 할당
            ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnCreateRoom();
            });
            _roomListTrigger.triggers.Add(ent);
        }


        // roomList를 받아와 roomList ui 생성 (버튼에 항상하여 사용)
        private void OnCreateRoomList() {
            _presenter.CreateRoomList();
        }

        // 방 생성 (버튼에 항상하여 사용)
        private void OnCreateRoom () {
            _presenter.CreateRoom();
        }
        /// <summary>
        /// 방 ui 생성
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userName"></param>
        internal void UpdateRoomUI() {
            ReadOnlyCollection<RoomData> roomDataList = _presenter.GetRoomList_RO();
            foreach (RoomData roomData in roomDataList) {

            }
        }
        /// <summary>
        /// main create room, room List 버튼 상태
        /// </summary>
        /// <param name="isActive"></param>
        private void ActiveMainUI() {
            
        }
    }
}
