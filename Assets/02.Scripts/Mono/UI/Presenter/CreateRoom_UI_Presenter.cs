//MVP Generater
using UnityEngine;
namespace Game.Mono.UI
{
	public class CreateRoom_UI_Presenter : UI_Presenter<CreateRoom_UI_View, CreateRoom_UI_Model>, IPresenter
	{
		internal void OpenCreateRoomPopup() {
			CreateRoom_UI_Popup popup = UI_Manager.Instance.InstancePopupUI<CreateRoom_UI_Popup>();
			string userName = _model.GetUserName();

            popup.UserName = userName == "" ? "User" : userName;
		}
    }
}
