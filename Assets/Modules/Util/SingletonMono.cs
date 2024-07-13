using UnityEngine;

namespace Utility
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        public static T Instance
        {
            get
            {
                s_instance ??= Instantiate(Resources.Load<T>("Singletons/DataPersistanceManager"));
                return s_instance;
            }
            private set
            {
                if (s_instance != null)
                {
                    Destroy(value.gameObject);
                }
                else
                {
                    s_instance = value;
                }
            }
        }

        [SerializeField]
        private bool _dontDestroyOnLoad;
        private static T s_instance;

        protected virtual void Awake()
        {
            Instance = this as T;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(Instance);
            }
        }
    }
}
