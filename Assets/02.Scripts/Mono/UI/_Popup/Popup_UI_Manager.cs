using UnityEngine;
using Game.DesignPattern;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
namespace Game.Mono.UI
{
    public enum POPUP_UI {
        ERR_UI,

    }
    public class Popup_UI_Manager : Singleton<Popup_UI_Manager>
    {
        private Canvas _mainCanvas;
        [SerializeField] AssetReference _UI_errPanel_ref;
        private void SetCanvas(Canvas canvas) {
            _mainCanvas = canvas;
        }

        public void GetPopupUI(POPUP_UI popup) {
            
       }
    }
}
