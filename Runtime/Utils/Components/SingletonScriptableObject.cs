using UnityEngine;

namespace LuckiusDev.Utils
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject
        where T : SingletonScriptableObject<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null) s_instance = Load();
                return s_instance;
            }
        }

        private static T Load()
        {
            T[] found = Resources.LoadAll<T>(string.Empty);

            if (found.Length == 0)
            {
                Debug.LogError($"[{typeof(T).Name}] No asset found. Create one inside a Resources folder.");
                return null;
            }

            if (found.Length > 1)
            {
                Debug.LogWarning($"[{typeof(T).Name}] {found.Length} assets found in Resources; using '{found[0].name}'.");
            }

            return found[0];
        }
    }
}
