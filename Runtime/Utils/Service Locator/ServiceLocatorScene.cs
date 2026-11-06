using UnityEngine;

namespace LuckiusDev.Utils
{
    [AddComponentMenu("LuckiusDev/Utils/Service Locator/Scene")]
    public sealed class ServiceLocatorScene : ServiceLocatorBootstrapper
    {
        protected override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}
