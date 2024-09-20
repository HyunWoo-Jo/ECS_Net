using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Bson;
using System;
using Game.DesignPattern;
using Cysharp.Threading.Tasks;
using Codice.CM.Common;
using System.Text;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
namespace Game.Network
{
    public class JoinServerManager : Singleton<JoinServerManager>
    {

        // join server info
        private int _port = 43233;
        private string _joinServerIP = "127.0.0.1";

        // tcp settings
        private TcpClient _tcpClient = null;
        private NetworkStream _stream;

        // listener
        private Action onConnectingListener; // 연결 완료
        private Action failConnectingListener; // 연결 실패
        
        private Action<string[]> roomListSusListener; // roomList 처리
        private Action roomListFailListener; // roomList 처리
        private Action<string[]> joinRoomSusListener; // join Room 처리
        private Action joinRoomFailListener; // join Room 처리
        private Action joinRoomNoRoomListener; // join Room 처리

        public void AddRoomListListener(Action<string[]> susAction, Action failAction) {
            roomListSusListener += susAction;
            roomListFailListener += failAction;
        }
        public void AddJoinRoomListener(Action<string[]> susAction, Action failAction, Action noRoomAction) {
            joinRoomSusListener += susAction;
            joinRoomFailListener += failAction;
            joinRoomNoRoomListener += noRoomAction;
        }

        public void AddOnConnectingListener(Action action) {
            onConnectingListener += action;
        }
        public void RemoveOnConnectingListener(Action action) { 
            onConnectingListener -= action; 
        }

        public void AddFailConnectingListener(Action action) {
            failConnectingListener += action;
        }
        public void RemoveFailConnectingListener(Action action) {
            failConnectingListener -= action;
        }


        private void OnDisable() {
            CloseTcp();
        }

        #region tcp funtion
        /// <summary>
        /// tcp 연결되어 있나 확인
        /// </summary>
        /// <returns></returns>
        public bool ChkConnected() {
            if(_tcpClient != null && _tcpClient.Connected) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// connect join server - client
        /// </summary>
        /// <returns></returns>
        public async void OnTcpCleintAsync() {
            _tcpClient = new TcpClient();
            try {
                await _tcpClient.ConnectAsync(_joinServerIP, _port).AsUniTask(); // 서버 연결
                _stream = _tcpClient.GetStream();               
                _ = ReadMessageAsync();
                onConnectingListener?.Invoke();
            } catch (Exception e) {
                failConnectingListener?.Invoke();
#if TESTING_DEBUG
                Debug.Log(e.Message);
#endif
            }
        }

        /// <summary>
        /// 비동기 수신 (반복)
        /// </summary>
        private async UniTask ReadMessageAsync() {
            StringBuilder strBulider = new ();
            byte[] buffer = new byte[1024];
            while (true) {
                try {
                    var nBytes = await _stream.ReadAsync(buffer, 0, buffer.Length).AsUniTask();
                    if (nBytes == 0) { // 스트림 종료 처리
                        break;
                    }
                    string msgPart = Encoding.UTF8.GetString(buffer, 0, nBytes);
                    strBulider.Append(msgPart);

                    string msg = strBulider.ToString();
                    int divIndex = msg.IndexOf('\n');

                    while (divIndex != -1) {
                        string completeMessage = strBulider.ToString(0, divIndex);
                        strBulider.Remove(0, divIndex + 1);
#if TESTING_DEBUG
                        Debug.Log("receive from join: " + completeMessage);
#endif
                        _ = MessageKernel(completeMessage);
                        divIndex = strBulider.ToString().IndexOf('\n');
                    }
                } catch (Exception e) {
#if TESTING_DEBUG
                    Debug.Log(e.Message);
#endif
                    break;
                }
            }
        }
        /// <summary>
        /// 비동기 송신
        /// </summary>
        /// <param name="msg"></param>
        public async UniTask SendMessageAsync(string msg) {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(msg + '\n');
                await _stream.WriteAsync(buffer, 0, buffer.Length);
#if TESTING_DEBUG
                Debug.Log("send to join: " + msg);
#endif
            } catch(Exception e) {
#if TESTING_DEBUG
                Debug.Log(e.Message);
#endif
            }
        }
        /// <summary>
        /// off 
        /// </summary>
        private void CloseTcp() {
            if (_stream != null) {
                _stream.Close();
                _stream.Dispose();
            }
            if (_tcpClient != null) {
                _tcpClient.Close();
                _tcpClient.Dispose();
            }      
        }
        #endregion

        #region logic
        private string[] MessageSplit(string msg) {
            return msg.Split(":"); 
        }
        private async UniTask MessageKernel(string msg) {
            await UniTask.RunOnThreadPool(() => {
                string[] splitMsg = msg.Split(":");
                switch (splitMsg[0]) {
                    case "msg":
                    MessageProceesor(splitMsg);
                    break;
                    case "data":
                    DataProcessor(splitMsg);
                    break;
                }
            });
        }
        /// <summary>
        /// msg 처리
        /// </summary>
        /// <param name="splitMsg"></param>
        private void MessageProceesor(string[] splitMsg) {
            
        }
        /// <summary>
        /// data 처리
        /// </summary>
        /// <param name="splitMsg"></param>
        private void DataProcessor(string[] splitMsg) {
            switch (splitMsg[1]) {
                case "roomList": // 방 목록 
                RoomListProcessor(splitMsg);
                break;
                case "joinRoom": // 방 접속
                JoinRoomProcessor(splitMsg);
                break;
            }
        }
        // 방 목록
        private void RoomListProcessor(string[] splitMsg) {
            string[] roomDatas;
            List<string> roomList = new List<string>();
            switch (splitMsg[2]) {
                case "sus": // 성공 / data:roomList:SoF:roomIpHash/roomName/userName/isPublic...
                roomDatas = splitMsg[2].Split("/");
                for (int i = 0; i < roomDatas.Length - 1; i += 4) {
                    roomList.Clear();
                    roomList.Add(roomDatas[i]);
                    roomList.Add(roomDatas[i + 1]);
                    roomList.Add(roomDatas[i + 2]);
                    roomList.Add(roomDatas[i + 3]);
                    roomListSusListener?.Invoke(roomList.ToArray());
                }
                break;
                case "noRoom": // 방이 없음
                roomListFailListener?.Invoke();
                break;
            }            
        }
        // 방 접속
        private void JoinRoomProcessor(string[] splitMsg) {
            string[] roomDatas;
            List<string> roomList = new List<string>();
            switch (splitMsg[2]) {
                case "sus": // 성공 / data:joinRoom:SoF:ip/port
                roomDatas = splitMsg[2].Split("/");
                roomList.Add(roomDatas[0]);
                roomList.Add(roomDatas[1]);
                joinRoomSusListener?.Invoke(roomList.ToArray());
                break;
                case "fail": // 실패
                joinRoomFailListener?.Invoke();
                break;
                case "noRoom": // 방이없음
                joinRoomNoRoomListener?.Invoke();
                break;
            }
        }
        #endregion

        #region command
        /// <summary>
        /// 방 생성
        /// </summary>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="roomName"></param>
        /// <param name="password"></param>
        public void CreateRoom(string port, string userName, string roomName, string password = "") {
            // 0.command / 1.type / 2.port / 3.userName
            // 4.roomName / 5.password
            string msg = string.Format("cmd:createRoom:{0}:{1}:{2}:{3}", port, userName, roomName, password);
            _ = SendMessageAsync(msg);
        }
        /// <summary>
        /// 방 요청
        /// </summary>
        public void RequestRoom() {
            string msg = "cmd:requestRoom";
            _ = SendMessageAsync(msg);
        }
        /// <summary>
        /// 방 접속
        /// </summary>
        /// <param name="roomHash"></param>
        /// <param name="password"></param>
        public void JoinRoom(string roomHash, string password = "") {
            string msg = string.Format("cmd:joinRoom:{0}:{1}", roomHash, password);
            _ = SendMessageAsync(msg);
        }


        #endregion

    }
}
