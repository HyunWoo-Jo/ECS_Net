using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Game.Utills;
namespace Game.Mono.UI
{
    public class Err_UI_Popup : Popup
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private EventTrigger _enterButton;

        



        public void UpdateText(string text) {
            _text.text = text;
        }


        internal override void OnOpen() {
            _enterButton.AddDownButton(OnClose); // ��ư�� ���� ��ư �߰�
        }

        internal override void OnClose() {
            base.OnClose();
        }

    }
}
