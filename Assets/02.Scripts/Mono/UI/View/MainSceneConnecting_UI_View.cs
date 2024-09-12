using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [SerializeField] private EventTrigger _serverClientTrigger;
        [SerializeField] private EventTrigger _clientTrigger;

        // Init
        private void Start() {
            // serverClientTrigger; / ��ư�� ��� �Ҵ�
            EventTrigger.Entry ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => { 
                OnServerClientAddListener(); 
            });
            _serverClientTrigger.triggers.Add(ent);

            // client Tigger / ��ư�� ��� �Ҵ�
            ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnClientAddListener();
            });
            _clientTrigger.triggers.Add(ent);
        }


        // button Server p2p ��Ȱ�ϴ� client ����
        private void OnServerClientAddListener() {
           // NetworkManager.Instance.LoadServerClient();
        }

        // button Client ����
        private void OnClientAddListener() {
           // NetworkManager.Instance.LoadClient();
        }
    }
}
