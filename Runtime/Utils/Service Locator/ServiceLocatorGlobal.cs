using UnityEngine;

namespace LuckiusDev.Utils
{
    [AddComponentMenu("LuckiusDev/Utils/Service Locator/Global")]
    public sealed class ServiceLocatorGlobal : ServiceLocatorBootstrapper
    {
        [SerializeField] bool dontDestroyOnLoad = true;

        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}
