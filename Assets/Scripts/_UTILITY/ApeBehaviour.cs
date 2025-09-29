namespace _UTILITY
{
    public abstract class ApeBehaviour
    {
        public bool Updating;
        protected ApeBehaviour()
        {
            //UpdateManager.DoUpdate += Update;
            //UpdateManager.DoFixedUpdate += FixedUpdate;
        }

        public virtual void OnEnable()
        {
            Updating = true;
            UpdateManager.DoUpdate += Update;
            UpdateManager.DoFixedUpdate += FixedUpdate;
        }
        public virtual void OnDisable()
        {
            //Updating = false;
            UpdateManager.DoUpdate -= Update;
            UpdateManager.DoFixedUpdate -= FixedUpdate;
        }


        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }
    }
}