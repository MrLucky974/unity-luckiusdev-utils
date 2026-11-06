using UnityEngine;

namespace LuckiusDev.Utils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class ServiceLocatorBootstrapper : MonoBehaviour
    {
        private ServiceLocator m_container;
        internal ServiceLocator Container => m_container.OrNull() ?? (m_container = GetComponent<ServiceLocator>());

        private bool m_hasBeenBootstrapped;

        private void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (m_hasBeenBootstrapped) return;
            m_hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}
