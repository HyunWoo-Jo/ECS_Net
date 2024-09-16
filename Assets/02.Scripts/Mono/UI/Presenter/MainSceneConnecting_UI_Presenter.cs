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
            JoinServerManager.Instance.roomListListener += AddRoomList; // �޽��� �� ����Ʈ ����
            JoinServerManager.Instance.joinRoomListener += JoinRoom; // �� ���� ����
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        internal void CreateRoom() {
            // sus
            JoinServerManager.Instance.onConnectingListener = () => {
                
            };
            // fail
            JoinServerManager.Instance.failConnectingListener = () => { 
            
            };
            JoinServerManager.Instance.OnTcpCleintAsync();

        }
        /// <summary>
        /// �� ��� ���� ����
        /// </summary>
        internal void CreateRoomList() {
            //sus
            
            //fail

        }
        /// <summary>
        /// room ��� ��ȯ
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return _model.roomDataList.AsReadOnly(); 
        }
       
        /// <summary>
        /// �� ���
        /// </summary>
        /// 0.roomHash, 1.roomName, 2.userName / 3.roomHash ...
        private void AddRoomList(string[] msg) {
            // model ����

            // ui ����
        }

        /// <summary>
        /// �� ����
        /// </summary>
        /// 0.ip, 1.port
        private void JoinRoom(string[] msg) {
            // sus

            // fail
            
        }

    }
}
 