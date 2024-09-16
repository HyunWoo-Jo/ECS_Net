using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Game.Network;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [SerializeField] private EventTrigger _createRoomTrigger;
        [SerializeField] private EventTrigger _roomListTrigger;

        // Init
        private void Start() {
            // create Room; / ��ư�� ��� �Ҵ�
            EventTrigger.Entry ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnCreateRoomList(); 
            });
            _createRoomTrigger.triggers.Add(ent);

            // room List / ��ư�� ��� �Ҵ�
            ent = new() {
                eventID = EventTriggerType.PointerDown
            };
            ent.callback.AddListener((data) => {
                OnCreateRoom();
            });
            _roomListTrigger.triggers.Add(ent);
        }


        // roomList�� �޾ƿ� roomList ui ���� (��ư�� �׻��Ͽ� ���)
        private void OnCreateRoomList() {
            // NetworkManager.Instance.LoadServerClient();
            _presenter.CreateRoomList();
        }

        // �� ���� (��ư�� �׻��Ͽ� ���)
        private void OnCreateRoom () {
            _presenter.CreateRoom();
           // NetworkManager.Instance.LoadClient();
        }
        /// <summary>
        /// �� ui ����
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userName"></param>
        private void CreateRoomUI() {

        }
        /// <summary>
        /// main create room, room List ��ư ����
        /// </summary>
        /// <param name="isActive"></param>
        private void ActiveMainUI() {
            
        }
    }
}
