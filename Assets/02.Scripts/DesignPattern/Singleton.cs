using UnityEngine;

namespace Game.DesignPattern
{
    [DefaultExecutionOrder(-100)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        private static T _instance;
        public static T Instance {
            get { return _instance; }
        }
       
        public virtual void Awake() {
            // init
            if (_instance == null) {
                _instance = this.gameObject.GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
