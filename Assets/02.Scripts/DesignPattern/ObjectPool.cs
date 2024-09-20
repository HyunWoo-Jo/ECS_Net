using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Game.DesignPattern
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _item;
        private readonly List<GameObject> _itemList = new();
        [SerializeField] private int _capacity;

        protected virtual void Awake() {
            while(_itemList.Count < _capacity) {
                AddItem();
            }    
        }
        /// <summary>
        /// ¹Þ¾Æ¿À±â
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private GameObject GetItem(bool isActive) {
            if (_itemList.Count == 0) {
                AddItem();
                _capacity++;
            }
            GameObject item = _itemList.First();
            _itemList.RemoveAt(0);
            item.SetActive(isActive);
            return item;
        }
        /// <summary>
        /// Ãß°¡
        /// </summary>
        private void AddItem() {
            GameObject item = GameObject.Instantiate(_item);
            _itemList.Add(item);
            item.AddComponent<ObjectPoolItem>().Init(this);
            item.SetActive(false);
            item.transform.SetParent(this.transform);
        }

        #region public
        /// <summary>
        /// ºô¸®±â
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public GameObject BorrowItem(bool isActive = true) {
            return GetItem(isActive);
        }
        /// <summary>
        /// ºô¸®±â
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public GameObject BorrowItem(Transform parent, bool isActive = true) {
            GameObject item = GetItem(isActive);
            item.transform.parent = parent;
            return item;
        }
        /// <summary>
        /// ºô¸®±â
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public T BorrowItem<T>(bool isActive = true) where T : MonoBehaviour {
            return BorrowItem(isActive).GetComponent<T>();
        }
        /// <summary>
        /// ºô¸®±â
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public T BorrowItem<T>(Transform parent, bool isActive = true) where T : MonoBehaviour {
            return BorrowItem(parent, isActive).GetComponent<T>();
        }
        /// <summary>
        /// ¹ÝÈ¯
        /// </summary>
        /// <param name="item"></param>
        public void RefundItem(GameObject item) {
            _itemList.Add(item);
            item.transform.SetParent(this.transform);
            item.SetActive(false);
        }
        #endregion
    }
}
