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
            _enterButton.AddDownButton(OnClose); // 버튼에 종료 버튼 추가
        }

        internal override void OnClose() {
            base.OnClose();
        }

    }
}
