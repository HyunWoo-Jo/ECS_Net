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
        UniTask loadState;
        private Dictionary<string, string> _key_Dic = new(); // addressable key가 저장되있는 딕셔너리 (key = script name)
        private Dictionary<string, List<GameObject>> _preLoad_Dic_List = new(); // 자주 사용하는 오브 젝트를 미리 로드하여 참조 보관하는 딕셔너리 리스트
        /// <summary>
        /// 자주 사용하는 prefab 추가
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        private void AddDicList(string key, GameObject obj) {
            if (!_preLoad_Dic_List.ContainsKey(key)) { // 키가 없을경우
                _preLoad_Dic_List.Add(key, new List<GameObject>());
            }
            _preLoad_Dic_List[key].Add(obj); // 추가
        }

        public override void Awake() {
            base.Awake();
            loadState = LoadCsv();
        }
        // key data 링크 csv를 로드 하는 코드
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
            Debug.Log("끝");
        }


        /// <summary>
        /// 에셋 로드후 생성
        /// </summary>
        /// <param name="key"></param>
        /// <param name="endAction"></param>
        public async void LoadInstance(string key, Action<GameObject> endAction) {
            await loadState;
            string addName = _key_Dic[key];
            GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(addName).ToUniTask();
            GameObject obj = GameObject.Instantiate(prefab);
            endAction(obj);
        }

        /// <summary>
        /// 자주 사용하는 에셋에 대한 로드
        /// </summary>
        public async void PreLoad(Label label) {
            string key = label.ToString();
            var loadObjList = await Addressables.LoadAssetsAsync<GameObject>(key).ToUniTask();
            for(int i =0;i< loadObjList.Count; i++) {
                AddDicList(key, loadObjList[i]);
            }
        }
        /// <summary>
        /// Instance한 리소스를 언로드
        /// </summary>
        /// <param name="key"></param>
        public void UnLoad(GameObject instanceObj) {
            Addressables.ReleaseInstance(instanceObj);
        }

        /// <summary>
        /// 자주 사용하는 에셋을 언로드
        /// </summary>
        /// <param name="label"></param>
        public void UnLoad(Label label) {
            string key = label.ToString();
            if (_preLoad_Dic_List.ContainsKey(key)) {
                List<GameObject> prefabList = _preLoad_Dic_List[key]; // 로드한 Prefab들을 받아옴
                _preLoad_Dic_List.Remove(key);
                for(int i =0;i< prefabList.Count; i++) {
                    Addressables.Release(prefabList[i]); // 해제
                }
                _preLoad_Dic_List.Clear(); // 메모리 해제
            }
        }

        
    }
}
