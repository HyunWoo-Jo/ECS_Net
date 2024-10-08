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


        private List<Popup> _popupList = new(); // ���� popup ���

        private Canvas FindMainCanvas() {
            return GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        }
        
        public GameObject LoadPrefab<T>() where T : ILoadAble {
            return ResourceManager.Instance.LoadInstance(typeof(T).Name);
        }

        /// <summary>
        /// prefab�� unload�ϴ� �Լ�
        /// </summary>
        /// <param name="prefab"></param>
        public void UnLoadPrefab(GameObject prefab) {
            ResourceManager.Instance.UnLoadPrefab(prefab);
        }

        /// <summary>
        /// UI ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actioon"></param>
        public GameObject InstanceUI<T>() where T : ILoadAble {
            GameObject obj = ResourceManager.Instance.LoadInstance(typeof(T).Name);
            Vector3 position = obj.transform.position;
            obj.transform.SetParent(_MainCanvas.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = position;
            return obj;
        }

        /// <summary>
        /// Popup ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public T InstancePopupUI<T>() where T : Popup {
            GameObject obj = InstanceUI<T>();
            T popup = obj.GetComponent<T>();
            popup.OnOpen();
            return popup;
        }
        /// <summary>
        /// popup ����
        /// </summary>
        /// <param name="popup"></param>
        internal void CloseUI(Popup popup) { // popup -> onClose �Լ��� ���ؼ� ����
            _popupList.Remove(popup);
            ResourceManager.Instance.UnLoad(popup.gameObject);
            Destroy(popup.gameObject);
        }

       internal void ShowErrPopup(string str) {
            Err_UI_Popup popup = InstancePopupUI<Err_UI_Popup>();
            popup.UpdateText(str);
        }
    }
}
