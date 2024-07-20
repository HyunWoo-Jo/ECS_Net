using Game.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Mono
{
    public class InitializeMainScene : MonoBehaviour
    {
        private bool isInit = false;

        private void Start()
        {
            NetworkManager.Instance.AddLoadListener(LoadPlayScene);
            isInit = true;
        }

        private void OnDisable() {
            if(isInit)
                NetworkManager.Instance.DeleteLoadListener(LoadPlayScene);
        }

        private void LoadPlayScene() {
            AsyncOperation asyncLoader =  SceneManager.LoadSceneAsync("PlayScene");
            NetworkManager.Instance.DeleteLoadListener(LoadPlayScene);
            isInit = false;
        }

    }
}
