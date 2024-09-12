using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Bson;
using System;
using Game.DesignPattern;
using Cysharp.Threading.Tasks;
using Codice.CM.Common;
using System.Text;
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
        public Action onConnectingListener; // 연결 완료
        public Action<string> readMsgListener; // 메시지 수신

        private void Start() {
            OnTcpCleintAsync();
        }

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
                await _tcpClient.ConnectAsync(_joinServerIP, _port).AsUniTask();
                _stream = _tcpClient.GetStream();
                onConnectingListener?.Invoke();
                ReadMessageAsync();
                await SendMessageAsync("cmd:requestRoom"); // test code
            } catch (Exception e) {
#if TESTING_DEBUG
                Debug.Log(e.Message);
#endif
            }
        }

        /// <summary>
        /// 비동기 수신 (반복)
        /// </summary>
        private async void ReadMessageAsync() {
            StringBuilder strBulider = new StringBuilder();
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

                    while(divIndex != -1) {
                        string completeMessage = strBulider.ToString(0, divIndex);
                        strBulider.Remove(0, divIndex + 1);
#if TESTING_DEBUG
                        Debug.Log("receive from join: " + completeMessage);
#endif
                        readMsgListener?.Invoke(completeMessage);
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
            if (_tcpClient != null) {
                _tcpClient.Close();
                _tcpClient.Dispose();
            }
            if(_stream != null) {
                _stream.Close();
                _stream.Dispose();
            }
            
        }

    }
}
