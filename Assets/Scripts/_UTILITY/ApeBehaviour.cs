namespace _UTILITY
{
    public abstract class ApeBehaviour
    {
        protected ApeBehaviour()
        {
            UpdateManager.DoUpdate += Update;
            UpdateManager.DoFixedUpdate += FixedUpdate;
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }
    }
}