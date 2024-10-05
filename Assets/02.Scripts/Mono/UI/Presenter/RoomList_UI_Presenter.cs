//MVP Generater
using Codice.CM.WorkspaceServer.DataStore;
using Game.Network;
using UnityEngine;
using System;
namespace Game.Mono.UI
{
	public class RoomList_UI_Presenter : UI_Presenter<RoomList_UI_View, RoomList_UI_Model>, IPresenter
	{
        private bool _isWork = false; // �ѹ��� ���� �ϵ��� üũ�ϴ� ����
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
                    ShowErrUI("������ ���ῡ �����߽��ϴ�.");
                    _isWork = false;
                });
                JoinServerManager.Instance.OnTcpCleintAsync();
            }
		}
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="msg"></param>
		private void SusRequest(string[] msg) {
            // model ���� Update Model
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
            // ui ����
            RoomList_UI_Popup popup =  UI_Manager.Instance.InstancePopupUI<RoomList_UI_Popup>();
            _view.ShowRoomUI(popup, _model.GetRoomDataList_RO()); // model�� �����͸� view�� ����
            _isWork = false;
        }
        /// <summary>
        /// ����������� (no Room)
        /// </summary>
		private void FailRequest() {
            ShowErrUI("�� ����� �������� �ʽ��ϴ�.");
            _isWork = false;
        }
        /// <summary>
        /// JoinServer�� �� ��û
        /// </summary>
        private void JoinServerRequestRoom() {
            JoinServerManager.Instance.RequestRoom();
        }
        


    }
}
