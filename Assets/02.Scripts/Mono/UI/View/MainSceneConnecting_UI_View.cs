using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.Mono.UI {
    public class MainSceneConnecting_UI_View : UI_View<MainSceneConnecting_UI_Presenter>
    {
        [SerializeField] private TMP_InputField _IpField;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private Button _serverClientButton;
        [SerializeField] private Button _clientButton;
    }
}
