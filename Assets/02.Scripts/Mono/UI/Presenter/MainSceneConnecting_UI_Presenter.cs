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
        private void Start() {
            // Init
            JoinServerManager.Instance.AddRoomListListener(AddRoomList); // �޽��� �� ����Ʈ ����
            JoinServerManager.Instance.AddJoinRoomListener(JoinRoom); // �� ���� ����
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        internal void CreateRoom() {
            // sus
            JoinServerManager.Instance.AddOnConnectingListener(() => {
                SusConnectingCreateRoom(); // �� ����
                JoinServerManager.Instance.RemoveOnConnectingListener(SusConnectingCreateRoom); // �ʱ�ȭ
            });
            // fail
            JoinServerManager.Instance.AddFailConnectingListener(() => { });        

            JoinServerManager.Instance.OnTcpCleintAsync();

        }
        /// <summary>
        /// �� ��� ���� ����
        /// </summary>
        internal void CreateRoomList() {
            // sus
            JoinServerManager.Instance.AddOnConnectingListener(() => {
                SusConeectingReqeustRoom(); // �� ��� ��û 
                JoinServerManager.Instance.RemoveOnConnectingListener(SusConeectingReqeustRoom); // �ʱ�ȭ
            });
            // fail
            JoinServerManager.Instance.AddFailConnectingListener(() => { });

            JoinServerManager.Instance.OnTcpCleintAsync();
        }
        /// <summary>
        /// room ��� ��ȯ
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return _model.GetRoomList_RO();
        }
       
        /// <summary>
        /// �޽ý� ���Ž� �� ���
        /// </summary>
        /// 0.roomHash, 1.roomName, 2.userName ,3.isPublic / 4.roomHash ...
        private void AddRoomList(string[] msg) {
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
        /// �� ����
        /// </summary>
        /// 0.ip, 1.port
        private void JoinRoom(string[] msg) {
            NetworkManager.Instance.SetIP(msg[0]);
            NetworkManager.Instance.SetPort(Convert.ToUInt16(msg[1]));
            NetworkManager.Instance.LoadClient();
        }
        /// <summary>
        /// ��Ʈ��ũ ���ῡ �����ϸ� �� ����
        /// </summary>
        private void SusConnectingCreateRoom() {
            int port = NetworkManager.Instance.GetUseAblePort(); // ��밡�� ��Ʈ �˻�
            if (port != -1) {
                NetworkManager.Instance.SetPort(Convert.ToUInt16(port));
                NetworkManager.Instance.LoadServerClient();
            } else {

            }
        }
        /// <summary>
        /// ��Ʈ��ũ ���ῡ �����ϸ� �� ��û
        /// </summary>
        private void SusConeectingReqeustRoom() {
            JoinServerManager.Instance.RequestRoom();
        }

    }
}
 