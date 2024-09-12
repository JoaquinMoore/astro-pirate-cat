using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;


namespace BuildSystem
{
    [System.Serializable]
    public class BuildingModel
    {
        public BuildingControler _controler { get; private set; }
        public Health _hpFuncion { get; private set; }
        //private AudioDataSO HitSond;
        //private AudioDataSO BreakSound;


        public BuildingModel(BuildSettings settings)
        {
            _controler = settings.Controler;

            if (settings.HPRef == null)
                return;

            _hpFuncion = settings.HPRef;
            _hpFuncion.OnDamage.AddListener(Damage);
            _hpFuncion.OnDie.AddListener(Death);
        }

        public virtual void VirtualUpdate() { }

        public virtual void SelectTask(GameObject obs) { }

        public virtual void Damage()
        {
            if (!_controler._base._Col.enabled)
                return;
            _controler._visual.TriggerHit();
            //_controler._visual.PlayAudio(HitSond);
        }

        public virtual void Death()
        {
            Debug.Log("heavy is dead");

            _controler._visual.Destroy(true);
            //_controler._visual.PlayAudio(BreakSound);
            if (_controler._base._Col != null)
                _controler._base._Col.enabled = false;
        }


        [System.Serializable]
        public class BuildSettings
        {
            public Health HPRef;
            public BuildingControler Controler;
        }


    }
}