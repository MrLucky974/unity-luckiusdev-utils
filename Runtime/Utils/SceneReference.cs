using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LuckiusDev.Utils
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private Object m_sceneAsset;
        [SerializeField] private string m_sceneName;

        public string SceneName => m_sceneName;

        public static implicit operator string(SceneReference @object) {
            return @object.SceneName;
        }

        public void Load()
        {
            SceneManager.LoadScene(m_sceneName);
        }

        public AsyncOperation LoadAsync()
        {
            return SceneManager.LoadSceneAsync(m_sceneName);
        }
    }
}