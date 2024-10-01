using UnityEngine;
using Game.DesignPattern;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Resources;
using Game.Mono;
using System;
using NUnit.Framework.Internal;
using Cysharp.Threading.Tasks.Triggers;
namespace Game.Mono.UI
{
    public enum POPUP_UI {
        ERR_UI,
        ROOMLIST_UI
    }
    public class UI_Manager : Singleton<UI_Manager>
    {
        private Canvas _mainCanvas;
        private Canvas _MainCanvas {
            get {
                if (_mainCanvas == null) {
                    _mainCanvas = FindMainCanvas();
                }
                return _mainCanvas;
            }
        }
        [Header("UI Ref")]
        [SerializeField] private AssetReference _UI_errPanel_ref;


        private List<Popup> _popupList = new(); // 현재 popup 목록
        private void Start() {
            InstancePopupUI<Err_UI_Popup>(null);
        }
        private Canvas FindMainCanvas() {
            return GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        }
        
        /// <summary>
        /// UI 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actioon"></param>
        public void InstanceUI<T>(Action<GameObject> action = null) where T : ILoadAble {
            ResourceManager.Instance.LoadInstance(typeof(T).Name, (obj) => {
                obj.transform.SetParent(_MainCanvas.transform);
                action?.Invoke(obj);
            });
        }

        /// <summary>
        /// Popup 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void InstancePopupUI<T>(Action<GameObject> action = null) where T : Popup {
            Debug.Log(typeof(T).Name);
            ResourceManager.Instance.LoadInstance(typeof(T).Name, (obj) => {
                obj.transform.SetParent(_MainCanvas.transform);
                obj.GetComponent<T>().OnOpen(); // open 함수 실행
                action?.Invoke(obj);
            });
        }
        /// <summary>
        /// popup 제거
        /// </summary>
        /// <param name="popup"></param>
        internal void CloseUI(Popup popup) { // popup -> onClose 함수를 통해서 접근
            _popupList.Remove(popup);
            ResourceManager.Instance.UnLoad(popup.gameObject);
            Destroy(popup.gameObject);
        }
    }
}
