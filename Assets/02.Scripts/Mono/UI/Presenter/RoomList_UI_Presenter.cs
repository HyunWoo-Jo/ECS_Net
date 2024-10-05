//MVP Generater
using Codice.CM.WorkspaceServer.DataStore;
using Game.Network;
using UnityEngine;
using System;
namespace Game.Mono.UI
{
	public class RoomList_UI_Presenter : UI_Presenter<RoomList_UI_View, RoomList_UI_Model>, IPresenter
	{
        private bool _isWork = false; // 한번만 동작 하도록 체크하는 변수
        private void Start() {
            // init
            JoinServerManager.Instance.AddRoomListListener(SusRequest, FailRequest);
        }

        internal void RequestRoom() {
            if (_isWork) return;
            _isWork = true;
            if (JoinServerManager.Instance.IsChkConnected()) {
                JoinServerRequestRoom();
            } else {
                JoinServerManager.Instance.AddOnConnectingListener(JoinServerRequestRoom);
                JoinServerManager.Instance.AddFailConnectingListener(() => { 
                    ShowErrUI("서버에 연결에 실패했습니다.");
                    _isWork = false;
                });
                JoinServerManager.Instance.OnTcpCleintAsync();
            }
		}
        /// <summary>
        /// 성공했을경우
        /// </summary>
        /// <param name="msg"></param>
		private void SusRequest(string[] msg) {
            // model 갱신 Update Model
            _model.ClearRoomDataList();
            for (int i = 0; i < msg.Length; i += 4) {
                RoomData roomData = new RoomData {
                    roomHash = msg[i],
                    roomName = msg[i + 1],
                    userName = msg[i + 2],
                    isPublic = Convert.ToBoolean(msg[i + 3])
                };
                _model.AddRoomData(roomData);
            }
            // ui 갱신
            RoomList_UI_Popup popup =  UI_Manager.Instance.InstancePopupUI<RoomList_UI_Popup>();
            _view.ShowRoomUI(popup, _model.GetRoomDataList_RO()); // model의 데이터를 view에 전달
            _isWork = false;
        }
        /// <summary>
        /// 실패했을경우 (no Room)
        /// </summary>
		private void FailRequest() {
            ShowErrUI("방 목록이 존재하지 않습니다.");
            _isWork = false;
        }
        /// <summary>
        /// JoinServer에 방 요청
        /// </summary>
        private void JoinServerRequestRoom() {
            JoinServerManager.Instance.RequestRoom();
        }
        


    }
}
