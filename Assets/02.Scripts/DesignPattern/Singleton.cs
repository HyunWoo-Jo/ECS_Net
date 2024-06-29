using UnityEngine;

namespace Game.DesignPattern
{
    public class Singleton<T> : MonoBehaviour
    {
        public static T Instance {
            get { return _instance; }
        }
        private static T _instance;


        public virtual void Awake() {
            // init
            if(_instance == null) {
                _instance = this.gameObject.GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }

    }
}
