using System;
using UnityEngine;

namespace _UTILITY
{
    public class UpdateManager : MonoBehaviour
    {
        private static UpdateManager s_instance;

        private static UpdateManager Instance
        {
            get
            {
                s_instance ??= new GameObject("UpdateManager").AddComponent<UpdateManager>();
                return s_instance;
            }
        }

        private void Update()
        {
            doUpdate.Invoke();
        }

        private void FixedUpdate()
        {
            doFixedUpdate.Invoke();
        }

        private event Action doUpdate;
        private event Action doFixedUpdate;

        public static event Action DoUpdate
        {
            add => Instance.doUpdate += value;
            remove => Instance.doUpdate -= value;
        }

        public static event Action DoFixedUpdate
        {
            add => Instance.doFixedUpdate += value;
            remove => Instance.doFixedUpdate -= value;
        }
    }
}