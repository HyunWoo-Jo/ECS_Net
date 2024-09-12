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
            // serverClientTrigger; / 버튼에 기능 할당
            EventTrigger.Entry ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => { 
                OnServerClientAddListener(); 
            });
            _serverClientTrigger.triggers.Add(ent);

            // client Tigger / 버튼에 기능 할당
            ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnClientAddListener();
            });
            _clientTrigger.triggers.Add(ent);
        }


        // button Server p2p 역활하는 client 생성
        private void OnServerClientAddListener() {
           // NetworkManager.Instance.LoadServerClient();
        }

        // button Client 연결
        private void OnClientAddListener() {
           // NetworkManager.Instance.LoadClient();
        }
    }
}
