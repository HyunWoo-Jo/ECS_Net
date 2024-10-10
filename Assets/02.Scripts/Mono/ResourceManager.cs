using UnityEngine;
using Game.DesignPattern;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace Game.Mono
{
    public class ResourceManager :Singleton<ResourceManager>
    {
        public enum Label {
            MainScene,
            Frequently
        }
        private Dictionary<string, string> _key_Dic = new(); // addressable key�� ������ִ� ��ųʸ� (key = script name)
        private Dictionary<string, List<GameObject>> _preLoad_Dic_List = new(); // ���� ����ϴ� ���� ��Ʈ�� �̸� �ε��Ͽ� ���� �����ϴ� ��ųʸ� ����Ʈ
        /// <summary>
        /// ���� ����ϴ� prefab �߰�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        private void AddDicList(string key, GameObject obj) {
            if (!_preLoad_Dic_List.ContainsKey(key)) { // Ű�� �������
                _preLoad_Dic_List.Add(key, new List<GameObject>());
            }
            _preLoad_Dic_List[key].Add(obj); // �߰�
        }

        public override void Awake() {
            base.Awake();
            _= LoadCsv();

        }
        // key data ��ũ csv�� �ε� �ϴ� �ڵ�
        private async UniTask LoadCsv() {
            string key = "addressableData.csv";
            var csv = await Addressables.LoadAssetAsync<TextAsset>(key).ToUniTask(); // open csv
            string[] lineStrs = csv.text.Split("\n");
            foreach(var line in lineStrs) {
                string[] keyAndData = line.Split(",");
                if (!string.IsNullOrEmpty(keyAndData[0])) {
                    _key_Dic.Add(keyAndData[0], keyAndData[1]);
                }
            }
        }
        public GameObject LoadPrefab(string key) {
            string addName = _key_Dic[key];
            return Addressables.LoadAssetAsync<GameObject>(addName).WaitForCompletion();
        }


        /// <summary>
        /// ���� �ε��� ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="endAction"></param>
        public GameObject LoadInstance(string key) {
            string addName = _key_Dic[key];
            GameObject prefab = Addressables.LoadAssetAsync<GameObject>(addName).WaitForCompletion();
            GameObject obj = GameObject.Instantiate(prefab);
            return obj;
        }

        /// <summary>
        /// ���� ����ϴ� ���¿� ���� �ε�
        /// </summary>
        public async void PreLoad(Label label) {
            string key = label.ToString();
            var loadObjList = await Addressables.LoadAssetsAsync<GameObject>(key).ToUniTask();
            for (int i = 0; i < loadObjList.Count; i++) {
                AddDicList(key, loadObjList[i]);
            }
        }
        /// <summary>
        /// prefab unload
        /// </summary>
        /// <param name="prefab"></param>
        public void UnLoadPrefab(GameObject prefab) {
            Addressables.Release(prefab);
        }

        /// <summary>
        /// Instance�� ���ҽ��� ��ε�
        /// </summary>
        /// <param name="key"></param>
        public void UnLoad(GameObject instanceObj) {
            Addressables.ReleaseInstance(instanceObj);
        }

        /// <summary>
        /// ���� ����ϴ� ������ ��ε�
        /// </summary>
        /// <param name="label"></param>
        public void UnLoad(Label label) {
            string key = label.ToString();
            if (_preLoad_Dic_List.ContainsKey(key)) {
                List<GameObject> prefabList = _preLoad_Dic_List[key]; // �ε��� Prefab���� �޾ƿ�
                _preLoad_Dic_List.Remove(key);
                for(int i =0;i< prefabList.Count; i++) {
                    Addressables.Release(prefabList[i]); // ����
                }
                _preLoad_Dic_List.Clear(); // �޸� ����
            }
        }

        
    }
}
