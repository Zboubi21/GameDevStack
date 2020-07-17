using UnityEngine;
using Sirenix.OdinInspector;

namespace GameDevStack.Patterns
{
    public class SingletonSerializedMonoBehaviour<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
    {
        private static T s_Instance;
        public static T Instance => s_Instance;

        [Header("Singleton Parameters")]

        [Tooltip("If true, the game object arn't be destroyed whend scene is unloaded")]
        [SerializeField] bool m_DontDestroyOnLoad = false;

        [Tooltip("Show an error if more than 1 Instance is detected")]
        [SerializeField] bool m_ShowInstanceError = false;

        protected virtual void Awake()
        {
            SetupSingleton();
        }
        protected virtual void SetupSingleton()
        {
            if(Instance == null)
            {
                s_Instance = this as T;
                if (m_DontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (m_ShowInstanceError)
                    Debug.LogError("Two instance of " + this);
                Destroy(gameObject);
            }
        }
    }
}