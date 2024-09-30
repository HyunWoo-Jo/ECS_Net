//MVP Generater
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Game.Mono.UI
{
	public class RoomList_UI_View : UI_View<RoomList_UI_Presenter>, IView
	{
		[Header("Button")]
		[SerializeField] private EventTrigger _roomListButton;

		[Header("Room Button UI")]
		[SerializeField] private GameObject _roomButtonUI;
		private List<GameObject> _roomButtonUIList = new();

		[Header("Room Button Panel")]
		[SerializeField] private GameObject _roomList_panel;
		[SerializeField] private Transform _content_tr;

        private void Start() {
			// init
			Assign2Button(_roomListButton, OnRoomListButton);
        }

		internal void ShowRoomUI(ReadOnlyCollection<RoomData> roomDataList_RO) {
			for (int i = 0; i < roomDataList_RO.Count; i++) {
				GameObject roomUi =	Instantiate(_roomButtonUI);
				// room tr Á¶Àý

			}
		}
		internal void HideRoom() {

		}


        #region button
        private void OnRoomListButton() {
			_presenter.RequestRoom();		
		}
        #endregion
    }
}
