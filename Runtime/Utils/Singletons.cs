using UnityEngine;

namespace LuckiusDev.Utils.Singleton
{
    /// <summary>
    /// A static instance is similar to a singleton, but instead of destroying any new
    /// instances, it overrides the current instance. This is handy for resetting the state
    /// and saves you of doing it manually.
    /// </summary>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }
        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Transforms a static instance into a basic singleton. This will destroy any new
    /// versions created, leaving the original instance intact.
    /// </summary>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            base.Awake();
        }
    }

    /// <summary>
    /// This will survive through scene loads. Perfect for system classes which require
    /// stateful, persistent data. Or audio sources where music plays through loading screens, etc...
    /// </summary>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
    
    /// <summary>
    /// A Singleton that auto-creates itself if no instance exists yet.
    /// </summary>
    public abstract class AutoSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        public new static T Instance
        {
            get
            {
                if (StaticInstance<T>.Instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name + " (AutoSingleton)");
                    StaticInstance<T>.Instance = singletonObject.AddComponent<T>();
                }
                return StaticInstance<T>.Instance;
            }
            protected set => StaticInstance<T>.Instance = value;
        }
    }
    
    /// <summary>
    /// A Persistent Singleton that auto-creates itself if no instance exists yet.
    /// </summary>
    public abstract class AutoPersistentSingleton<T> : PersistentSingleton<T> where T : MonoBehaviour
    {
        public new static T Instance
        {
            get
            {
                if (StaticInstance<T>.Instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name + " (AutoPersistentSingleton)");
                    StaticInstance<T>.Instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
                return StaticInstance<T>.Instance;
            }
            protected set => StaticInstance<T>.Instance = value;
        }
    }
}

// PersistentSingleton<T> - Order of execution :
//if (Instance != null && Instance != this)
//{
//    Destroy(gameObject);
//    return;
//}
// Instance = this as T;
// DontDestroyOnLoad(gameObject);