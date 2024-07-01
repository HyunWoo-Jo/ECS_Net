using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [SerializeField] private TMP_InputField _IpField;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private EventTrigger _serverClientTrigger;
        [SerializeField] private EventTrigger _clientTrigger;

        // Init
        private void Start() {
            _IpField.onValueChanged.AddListener(IpChangeAddListener);
            _portField.onValueChanged.AddListener(PortChangeAddListener);

            // serverClientTrigger;
            EventTrigger.Entry ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => { 
                OnServerClientAddListener(); 
            });
            _serverClientTrigger.triggers.Add(ent);

            // client Tigger
            ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnClientAddListener();
            });
            _clientTrigger.triggers.Add(ent);
        }

        // ip ��ȿ�� �˻� �� ��ȿ �Ҷ��� NetworkManager�� �Ҵ�
        private void IpChangeAddListener(string str) {
            _presenter.IpVarification(str);
            _IpField.text = str;

        }
        // port ��ȿ�� �˻� �� NetworkManager�� �Ҵ� / ��ȿ text�� ����
        private void PortChangeAddListener(string str) {
            string text =_presenter.PortVarification(str);
            _portField.text = text;
        }
       
        private void OnServerClientAddListener() {
            NetworkManager.Instance.LoadServerClient();
        }

        private void OnClientAddListener() {
            NetworkManager.Instance.LoadClient();
        }
    }
}
