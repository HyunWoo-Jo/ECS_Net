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

		private GameObject _roomButtonUI_prefab;
        private void Start() {
			// init
			Assign2Button(_roomListButton, OnRoomListButton);
        }

        private void OnDisable() {
			if (_roomButtonUI_prefab != null) {
				UI_Manager.Instance.UnLoadPrefab(_roomButtonUI_prefab);
			}
        }
        internal void ShowRoomUI(RoomList_UI_Popup popup, ReadOnlyCollection<RoomData> roomDataList_RO) {
			if(_roomButtonUI_prefab == null) _roomButtonUI_prefab = UI_Manager.Instance.LoadPrefab<RoomButton_UI>(); // load
            for (int i = 0; i < roomDataList_RO.Count; i++) {
				GameObject roomUi =	Instantiate(_roomButtonUI_prefab);
				// room tr Á¶Àý
				roomUi.transform.SetParent(popup.GetContentTr());
				RectTransform rectTr = roomUi.GetComponent<RectTransform>();
				rectTr.localScale = Vector3.one;
				float height = rectTr.sizeDelta.y;
				float half = height * 0.5f;
                rectTr.localPosition = new Vector3(0, -height * i -(20 *(i +1)) - half, 0);
				// setting
				RoomButton_UI roomButton_UI = roomUi.GetComponent<RoomButton_UI>();
				roomButton_UI.UpdateRoomData(roomDataList_RO[i]);

			}
		}

        #region button
        private void OnRoomListButton() {
			_presenter.RequestRoom();		
		}
        #endregion
    }
}
