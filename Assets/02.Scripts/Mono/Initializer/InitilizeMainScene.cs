using Game.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Mono
{
    public class InitilizeMainScene : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Instance.AddLoadListener(LoadPlayScene);
            
        }

        private void OnDisable() {
            NetworkManager.Instance.DeleteLoadListener(LoadPlayScene);
        }

        private void LoadPlayScene() {
            SceneManager.LoadScene("PlayScene");
        }

    }
}
