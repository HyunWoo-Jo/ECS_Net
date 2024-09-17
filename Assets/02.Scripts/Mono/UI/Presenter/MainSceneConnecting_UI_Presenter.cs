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
            JoinServerManager.Instance.AddRoomListListener(AddRoomList); // 메시지 룸 리스트 수신
            JoinServerManager.Instance.AddJoinRoomListener(JoinRoom); // 방 접속 수신
        }

        /// <summary>
        /// 방 생성 로직
        /// </summary>
        internal void CreateRoom() {
            // sus
            JoinServerManager.Instance.AddOnConnectingListener(() => {
                SusConnectingCreateRoom(); // 방 생성
                JoinServerManager.Instance.RemoveOnConnectingListener(SusConnectingCreateRoom); // 초기화
            });
            // fail
            JoinServerManager.Instance.AddFailConnectingListener(() => { });        

            JoinServerManager.Instance.OnTcpCleintAsync();

        }
        /// <summary>
        /// 방 목록 생성 로직
        /// </summary>
        internal void CreateRoomList() {
            // sus
            JoinServerManager.Instance.AddOnConnectingListener(() => {
                SusConeectingReqeustRoom(); // 방 목록 요청 
                JoinServerManager.Instance.RemoveOnConnectingListener(SusConeectingReqeustRoom); // 초기화
            });
            // fail
            JoinServerManager.Instance.AddFailConnectingListener(() => { });

            JoinServerManager.Instance.OnTcpCleintAsync();
        }
        /// <summary>
        /// room 목록 반환
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return _model.GetRoomList_RO();
        }
       
        /// <summary>
        /// 메시시 수신시 방 등록
        /// </summary>
        /// 0.roomHash, 1.roomName, 2.userName ,3.isPublic / 4.roomHash ...
        private void AddRoomList(string[] msg) {
            // model 갱신
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
            // ui 갱신
            _view.UpdateRoomUI();
        }

        /// <summary>
        /// 방 연결
        /// </summary>
        /// 0.ip, 1.port
        private void JoinRoom(string[] msg) {
            NetworkManager.Instance.SetIP(msg[0]);
            NetworkManager.Instance.SetPort(Convert.ToUInt16(msg[1]));
            NetworkManager.Instance.LoadClient();
        }
        /// <summary>
        /// 네트워크 연결에 성공하면 방 생성
        /// </summary>
        private void SusConnectingCreateRoom() {
            int port = NetworkManager.Instance.GetUseAblePort(); // 사용가능 포트 검색
            if (port != -1) {
                NetworkManager.Instance.SetPort(Convert.ToUInt16(port));
                NetworkManager.Instance.LoadServerClient();
            } else {

            }
        }
        /// <summary>
        /// 네트워크 연결에 성공하면 방 요청
        /// </summary>
        private void SusConeectingReqeustRoom() {
            JoinServerManager.Instance.RequestRoom();
        }

    }
}
 