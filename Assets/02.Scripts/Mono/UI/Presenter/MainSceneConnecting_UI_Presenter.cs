using JetBrains.Annotations;
using UnityEngine;
using System;
using Game.Network;
using System.Net;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Mono.UI {
    // 내부를 internal로 설계
    public class MainSceneConnecting_UI_Presenter : UI_Presenter<MainSceneConnecting_UI_View, MainSceneConnecting_UI_Model>
    {
        private void Start() {
            // Init
            JoinServerManager.Instance.roomListListener += AddRoomList; // 메시지 룸 리스트 수신
            JoinServerManager.Instance.joinRoomListener += JoinRoom; // 방 접속 수신
        }

        /// <summary>
        /// 방 생성 로직
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
        /// 방 목록 생성 로직
        /// </summary>
        internal void CreateRoomList() {
            //sus
            
            //fail

        }
        /// <summary>
        /// room 목록 반환
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return _model.roomDataList.AsReadOnly(); 
        }
       
        /// <summary>
        /// 방 등록
        /// </summary>
        /// 0.roomHash, 1.roomName, 2.userName / 3.roomHash ...
        private void AddRoomList(string[] msg) {
            // model 갱신

            // ui 갱신
        }

        /// <summary>
        /// 방 연결
        /// </summary>
        /// 0.ip, 1.port
        private void JoinRoom(string[] msg) {
            // sus

            // fail
            
        }

    }
}
 