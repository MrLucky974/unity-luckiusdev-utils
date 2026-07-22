using System;
using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuckiusDev.Utils.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator s_global;
        private static Dictionary<Scene, ServiceLocator> s_sceneContainers;
        private static List<GameObject> s_tmpSceneGameObjects;

        private readonly ServiceManager m_services = new();

        const string k_globalServiceLocatorName = "ServiceLocator [Global]";
        const string k_sceneServiceLocatorName = "ServiceLocator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (s_global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global", this);
            }
            else if (s_global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
            }
            else
            {
                s_global = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (s_sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", this);
                return;
            }

            s_sceneContainers.Add(scene, this);
        }

        /// <summary>
        /// Gets the global ServiceLocator instance. Creates new if none exists.
        /// </summary>        
        public static ServiceLocator Global
        {
            get
            {
                if (s_global != null) return s_global;

                if (FindFirstObjectByType<ServiceLocatorGlobal>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return s_global;
                }

                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobal>().BootstrapOnDemand();

                return s_global;
            }
        }

        /// <summary>
        /// Returns the <see cref="ServiceLocator"/> configured for the scene of a MonoBehaviour. Falls back to the global instance.
        /// </summary>
        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if (s_sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != mb)
            {
                return container;
            }

            s_tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(s_tmpSceneGameObjects);

            foreach (GameObject go in s_tmpSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorScene>() != null))
            {
                if (go.TryGetComponent(out ServiceLocatorScene bootstrapper) && bootstrapper.Container != mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return Global;
        }

        /// <summary>
        /// Gets the closest ServiceLocator instance to the provided 
        /// MonoBehaviour in hierarchy, the ServiceLocator for its scene, or the global ServiceLocator.
        /// </summary>
        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
        }

        /// <summary>
        /// Registers a service to the ServiceLocator using the service's type.
        /// </summary>
        /// <param name="service">The service to register.</param>  
        /// <typeparam name="T">Class type of the service to be registered.</typeparam>
        /// <returns>The ServiceLocator instance after registering the service.</returns>
        public ServiceLocator Register<T>(T service)
        {
            m_services.Register(service);
            return this;
        }

        /// <summary>
        /// Registers a service to the ServiceLocator using a specific type.
        /// </summary>
        /// <param name="type">The type to use for registration.</param>
        /// <param name="service">The service to register.</param>  
        /// <returns>The ServiceLocator instance after registering the service.</returns>
        public ServiceLocator Register(Type type, object service)
        {
            m_services.Register(type, service);
            return this;
        }

        /// <summary>
        /// Gets a service of a specific type. If no service of the required type is found, an error is thrown.
        /// </summary>
        /// <param name="service">Service of type T to get.</param>  
        /// <typeparam name="T">Class type of the service to be retrieved.</typeparam>
        /// <returns>The ServiceLocator instance after attempting to retrieve the service.</returns>
        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;

            if (!TryGetNextInHierarchy(out ServiceLocator container))
                throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} not registered");
            
            container.Get(out service);
            return this;
        }

        /// <summary>
        /// Tries to get a service of a specific type. Returns whether or not the process is successful.
        /// </summary>
        /// <param name="service">Service of type T to get.</param>  
        /// <typeparam name="T">Class type of the service to be retrieved.</typeparam>
        /// <returns>True if the service retrieval was successful, false otherwise.</returns>
        public bool TryGet<T>(out T service) where T : class
        {
            service = null;

            if (TryGetService(out service))
                return true;

            return TryGetNextInHierarchy(out ServiceLocator container) && container.TryGet(out service);
        }

        private bool TryGetService<T>(out T service) where T : class
        {
            return m_services.TryGet(out service);
        }

        private bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == s_global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
            return container != null;
        }

        private void OnDestroy()
        {
            if (this == s_global)
            {
                s_global = null;
            }
            else if (s_sceneContainers.ContainsValue(this))
            {
                s_sceneContainers.Remove(gameObject.scene);
            }
        }

        // https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            s_global = null;
            s_sceneContainers = new Dictionary<Scene, ServiceLocator>();
            s_tmpSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Service Locator/Add Global")]
        private static void AddGlobal()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobal));
        }

        [MenuItem("GameObject/Service Locator/Add Scene")]
        private static void AddScene()
        {
            var go = new GameObject(k_sceneServiceLocatorName, typeof(ServiceLocatorScene));
        }
#endif
    }
}
