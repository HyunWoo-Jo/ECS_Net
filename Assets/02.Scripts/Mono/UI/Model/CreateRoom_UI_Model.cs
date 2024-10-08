//MVP Generater
using Game.Utills;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
namespace Game.Mono.UI
{
	public class CreateRoom_UI_Model : UI_Model<CreateRoom_UI_Presenter>, IModel
	{
		[SerializeField] private TMP_InputField _userName;

		internal string GetUserName() {
			return _userName.text;
		}

    }
}
