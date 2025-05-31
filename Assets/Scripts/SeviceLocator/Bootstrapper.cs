using UnityEngine;

namespace UnityServiceLocator
{
    [DisallowMultipleComponent]
    public abstract class Bootstrapper : ServiceLocator
    {
        private bool hasBeenBootstrapped;

        private void Awake()
        {
            BootstrapOnDemand();
        }

        public void BootstrapOnDemand()
        {
            if (hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}