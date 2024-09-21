using JetBrains.Annotations;
using UnityEngine;
using System;
using Game.Network;
using System.Net;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Mono.UI {
    // ���θ� internal�� ����
    public class MainSceneConnecting_UI_Presenter : UI_Presenter<MainSceneConnecting_UI_View, MainSceneConnecting_UI_Model>
    {
        private bool isTryConnecting = false; // ������ �ѹ��� �õ� �ǵ��� �����ϴ� bool
        private void Start() {
            // Init
            JoinServerManager.Instance.AddRoomListListener(AddRoomListSus, ShowNoRoom); // �޽��� �� ����Ʈ ����
            JoinServerManager.Instance.AddJoinRoomListener(JoinRoomSus, JoinRoomFail, ShowNoRoom); // �� ���� ����
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        internal void CreateRoom() {   
            if (JoinServerManager.Instance.ChkConnected()) { // ���� �Ǿ� �������
                SusConnectingCreateRoom();
            } else if(!isTryConnecting) { // ���� �� �Ǿ� �������
                // sus
                JoinServerManager.Instance.AddOnConnectingListener(() => {
                    SusConnectingCreateRoom(); // �� ����
                    JoinServerManager.Instance.RemoveOnConnectingListener(SusConnectingCreateRoom); // �ʱ�ȭ
                    isTryConnecting = false;
                });
                // fail
                JoinServerManager.Instance.AddFailConnectingListener(() => {
                    isTryConnecting = false;
                    _view.ShowErrText("������ ������ �����Ͽ����ϴ�.");
                });
                isTryConnecting = true;
                JoinServerManager.Instance.OnTcpCleintAsync();
            }

        }
        /// <summary>
        /// �� ��� ���� ����
        /// </summary>
        internal void CreateRoomList() {
            
            if (JoinServerManager.Instance.ChkConnected()) { // ���� �Ǿ� �������
                SusConeectingReqeustRoom();
            } else if (!isTryConnecting) { // ���� �� �Ǿ� �������
                // sus
                JoinServerManager.Instance.AddOnConnectingListener(() => {
                    SusConeectingReqeustRoom(); // �� ��� ��û 
                    JoinServerManager.Instance.RemoveOnConnectingListener(SusConeectingReqeustRoom); // �ʱ�ȭ
                    isTryConnecting = false;
                });
                // fail
                JoinServerManager.Instance.AddFailConnectingListener(() => { 
                    isTryConnecting = false;
                    _view.ShowErrText("������ ������ �����Ͽ����ϴ�.");
                });
                isTryConnecting = true;
                JoinServerManager.Instance.OnTcpCleintAsync();
            }
        }
        /// <summary>
        /// room ��� ��ȯ
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return _model.GetRoomList_RO();
        }

        /// <summary>
        /// ��Ʈ��ũ ���ῡ �����ϸ� �� ����
        /// </summary>
        private void SusConnectingCreateRoom() {
            int port = NetworkManager.Instance.GetUseAblePort(); // ��밡�� ��Ʈ �˻�
            if (port != -1) { // ��� ���� ��Ʈ�� ����
                NetworkManager.Instance.SetPort(Convert.ToUInt16(port));
                NetworkManager.Instance.LoadServerClient();
                JoinServerManager.Instance.CreateRoom(port.ToString(), "test", "roomdName");
            } else { 
                _view.ShowErrText("��� ������ ��Ʈ�� �������� �ʽ��ϴ�");
            }
        }
        /// <summary>
        /// ��Ʈ��ũ ���ῡ �����ϸ� �� ��û
        /// </summary>
        private void SusConeectingReqeustRoom() {
            JoinServerManager.Instance.RequestRoom();
        }

        #region Logic

        /// <summary>
        /// �޽ý� ���Ž� �� ���
        /// </summary>
        /// 0.roomHash, 1.roomName, 2.userName ,3.isPublic / 4.roomHash ...
        private void AddRoomListSus(string[] msg) {
            // model ����
            _model.ClearRoomData();
            for (int i = 0; i < msg.Length; i += 4) {
                RoomData roomData = new RoomData {
                    roomHash = msg[i],
                    roomName = msg[i + 1],
                    userName = msg[i + 2],
                    isPublic = Convert.ToBoolean(msg[i + 3])
                };
                _model.AddRoomDataList(roomData);
            }
            // ui ����
            _view.UpdateRoomUI();
        }
        /// <summary>
        /// Room�� �������� ����
        /// </summary>
        private void ShowNoRoom() {
            _view.ShowErrText("���� �������� �ʽ��ϴ�.");
        }
        /// <summary>
        /// �� ����
        /// </summary>
        /// 0.ip, 1.port
        private void JoinRoomSus(string[] msg) {
            NetworkManager.Instance.SetIP(msg[0]);
            NetworkManager.Instance.SetPort(Convert.ToUInt16(msg[1]));
            NetworkManager.Instance.LoadClient();
        }
        private void JoinRoomFail() {
            _view.ShowErrText("��� ��ȣ�� �ٸ��ϴ�.");
        }

        #endregion
    }
}
 