using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Game.Network;
using System;
using Game.Utills;
namespace Game.Mono.UI
{
    public class CreateRoom_UI_Popup : Popup
    {
        private string _userName;
        public string UserName { 
            set { _userName = value; } 
        }


        [SerializeField] private TMP_InputField _roomNameText;
        [SerializeField] private TMP_InputField _passwordText;
        [SerializeField] private EventTrigger _backButton;
        [SerializeField] private EventTrigger _EnterButton;

        internal override void OnOpen() {
            _backButton.AddDownButton(OnBackButton);
            _EnterButton.AddDownButton(OnEnterButton);
        }
        internal override void OnClose() {
            base.OnClose();
        }

        private void OnBackButton() {
            OnClose();
        }

        private void OnEnterButton() {
            int port = NetworkManager.Instance.GetUseAblePort(); // ��밡�� ��Ʈ �˻�
            if (port != -1) { // ��� ���� ��Ʈ�� ����
                NetworkManager.Instance.SetPort(Convert.ToUInt16(port));
                NetworkManager.Instance.LoadServerClient();
                JoinServerManager.Instance.CreateRoom(port.ToString(), _userName, _roomNameText.text , _passwordText.text);
            } else {
                ShowErrUI("��� ������ ��Ʈ�� �������� �ʽ��ϴ�");
            }
        }
    }
}
