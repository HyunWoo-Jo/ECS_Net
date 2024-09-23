using UnityEngine;
using Game.DesignPattern;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
namespace Game.Mono
{
    public static class AddressablesExtension {
        // AsyncOperation -> UniTask
        private static async UniTask<T> ToUniTask<T>(this AsyncOperationHandle<T> handle) {
            UniTaskCompletionSource<T> completionSource = new();

            handle.Completed += op => completionSource.TrySetResult(op.Result);
            handle.Completed += op => completionSource.TrySetException(op.OperationException);

            return await completionSource.Task;
        }
    }


    public class ResourceManager :Singleton<ResourceManager>
    {
        

        private async void Instantiate(string key, Action<GameObject> endAction) {
            GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(key).ToUniTask();
            GameObject obj = Instantiate(prefab);
            endAction(obj);
        }
    }
}
