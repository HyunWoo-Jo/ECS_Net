//MVP Generater
using Game.Utills;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Mono.UI
{
	public class CreateRoom_UI_View : UI_View<CreateRoom_UI_Presenter>, IView
	{
        [SerializeField] private EventTrigger _createButton;
        protected override void Awake() {
            base.Awake();
            _createButton.AddDownButton(OnCreateButton);
        }

        private void OnCreateButton() {
            _presenter.OpenCreateRoomPopup();
        }
    }
}
