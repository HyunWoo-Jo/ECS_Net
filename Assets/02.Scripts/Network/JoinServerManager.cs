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
using PimDeWitte.UnityMainThreadDispatcher;
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
        private Action _onConnectingListener; // ���� �Ϸ�
        private Action _failConnectingListener; // ���� ����
        
        private Action<string[]> _roomListSusListener; // roomList ó��
        private Action _roomListFailListener; // roomList ó��
        private Action<string[]> _joinRoomSusListener; // join Room ó��
        private Action _joinRoomFailListener; // join Room ó��
        private Action _joinRoomNoRoomListener; // join Room ó��

        // bool
        private bool _isWork = false; // ����, ���� �� �۾����ΰ� Ȯ��
        public bool IsWork { get { return _isWork; } }
        public void AddRoomListListener(Action<string[]> susAction, Action failAction) {
            _roomListSusListener += susAction;
            _roomListFailListener += failAction;
        }
        public void AddJoinRoomListener(Action<string[]> susAction, Action failAction, Action noRoomAction) {
            _joinRoomSusListener += susAction;
            _joinRoomFailListener += failAction;
            _joinRoomNoRoomListener += noRoomAction;
        }
        /// <summary>
        /// ���� ���� ������ �ѹ��� �۵�
        /// </summary>
        /// <param name="action"></param>
        public void AddOnConnectingListener(Action action) {
            _onConnectingListener += action;
        }
        /// <summary>
        /// ���� ���� ���н� �ѹ��� �۵�
        /// </summary>
        /// <param name="action"></param>
        public void RemoveOnConnectingListener(Action action) { 
            _onConnectingListener -= action; 
        }

        public void AddFailConnectingListener(Action action) {
            _failConnectingListener += action;
        }
        public void RemoveFailConnectingListener(Action action) {
            _failConnectingListener -= action;
        }


        private void OnDisable() {
            CloseTcp();

        }
        private void OnApplicationQuit() { // ����� �ʱ�ȭ
            _onConnectingListener = null;
            _failConnectingListener = null;
            _roomListSusListener = null;
            _roomListFailListener = null;
            _joinRoomSusListener = null;
            _joinRoomFailListener = null;
            _joinRoomNoRoomListener = null;
        }

        #region tcp funtion
        /// <summary>
        /// tcp ����Ǿ� �ֳ� Ȯ��
        /// </summary>
        /// <returns></returns>
        public bool IsChkConnected() {
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
            _isWork = true;
            try {
                await _tcpClient.ConnectAsync(_joinServerIP, _port).AsUniTask(); // ���� ����
                _stream = _tcpClient.GetStream();               
                _ = ReadMessageAsync();
                _onConnectingListener?.Invoke();                
            } catch (Exception e) {
                _failConnectingListener?.Invoke();
                
#if TESTING_DEBUG
                Debug.Log(e.Message);
#endif
            }
            _onConnectingListener = null;
            _failConnectingListener = null;
            _isWork = false;
        }

        /// <summary>
        /// �񵿱� ���� (�ݺ�)
        /// </summary>
        private async UniTask ReadMessageAsync() {
            StringBuilder strBulider = new ();
            byte[] buffer = new byte[1024];
            while (true) {
                try {
                    var nBytes = await _stream.ReadAsync(buffer, 0, buffer.Length).AsUniTask();
                    if (nBytes == 0) { // ��Ʈ�� ���� ó��
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
                _isWork = false;
            }
        }
        /// <summary>
        /// �񵿱� �۽�
        /// </summary>
        /// <param name="msg"></param>
        public async UniTask SendMessageAsync(string msg) {
            try {
                _isWork = true; // �������� ������ ���� ReadMessageAsync���� false ó��
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
        /// msg ó��
        /// </summary>
        /// <param name="splitMsg"></param>
        private void MessageProceesor(string[] splitMsg) {
            
        }
        /// <summary>
        /// data ó��
        /// </summary>
        /// <param name="splitMsg"></param>
        private void DataProcessor(string[] splitMsg) {
            switch (splitMsg[1]) {
                case "roomList": // �� ��� 
                RoomListProcessor(splitMsg);
                break;
                case "joinRoom": // �� ����
                JoinRoomProcessor(splitMsg);
                break;
            }
        }
        // �� ���
        private void RoomListProcessor(string[] splitMsg) {
            string[] roomDatas;
            List<string> roomList = new();
            switch (splitMsg[2]) {
                case "sus": // ���� / data:roomList:SoF:roomIpHash/roomName/userName/isPublic...
                roomDatas = splitMsg[3].Split("/");
                for (int i = 0; i < roomDatas.Length - 1; i += 4) {
                    roomList.Clear();
                    roomList.Add(roomDatas[i]);
                    roomList.Add(roomDatas[i + 1]);
                    roomList.Add(roomDatas[i + 2]);
                    roomList.Add(roomDatas[i + 3]);
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        _roomListSusListener?.Invoke(roomList.ToArray());
                    });
                }
                break;
                case "noRoom": // ���� ����
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    _roomListFailListener?.Invoke();
                });
                break;
            }            
        }
        // �� ����
        private void JoinRoomProcessor(string[] splitMsg) {
            string[] roomDatas;
            List<string> roomList = new List<string>();
            switch (splitMsg[2]) {
                case "sus": // ���� / data:joinRoom:SoF:ip/port
                roomDatas = splitMsg[2].Split("/");
                roomList.Add(roomDatas[0]);
                roomList.Add(roomDatas[1]);
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    _joinRoomSusListener?.Invoke(roomList.ToArray());
                });
                break;
                case "fail": // ����
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    _joinRoomFailListener?.Invoke();
                });
                break;
                case "noRoom": // ���̾���
                UnityMainThreadDispatcher.Instance().Enqueue(() => {       
                    _joinRoomNoRoomListener?.Invoke();
                });
                break;
            }
        }
        #endregion

        #region command
        /// <summary>
        /// �� ����
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
        /// �� ��û
        /// </summary>
        public void RequestRoom() {
            string msg = "cmd:requestRoom";
            _ = SendMessageAsync(msg);
        }
        /// <summary>
        /// �� ����
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
