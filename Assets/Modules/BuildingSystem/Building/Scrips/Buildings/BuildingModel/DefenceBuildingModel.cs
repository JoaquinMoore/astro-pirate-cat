using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class DefenceBuildingModel : BuildingModel
    {
        RepairFunc _repairFuncion;
        MountingFunc _mountinFuncion;

        public DefenceBuildingModel(Settings settings) : base(settings)
        {
            _repairFuncion = new(settings.RepairSettings, settings.Controler);
            _mountinFuncion = new(settings.MountSettings, settings.Controler);
        }


        public override void VirtualUpdate()
        {
            if (_controler._base._Col.enabled)
                _mountinFuncion.CheckMounted();
        }


        public override void SelectTask(GameObject obs)
        {
            if (_controler._base._Col.enabled)
            {
                _mountinFuncion.SetCrew(obs);
                //SetCrew(member);
                //task.Finish();
                return;
            }



            //member.CurrentPosition = transform.position + (Vector3)(Random.insideUnitCircle * 2);
            //StopAllCoroutines();
            _repairFuncion.StartRepairs(obs);
            //StartCoroutine(Repair(task));
        }

        [System.Serializable]
        public class Settings : BuildSettings
        {
            public RepairFunc.Settings RepairSettings;
            public MountingFunc.Settings MountSettings;

        }


    }
}