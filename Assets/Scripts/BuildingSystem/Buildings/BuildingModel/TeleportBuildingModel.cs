using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class TeleportBuildingModel : BuildingModel
    {
        RepairFunc _repairFunciont;
        public TeleportBuildingModel(Settings settings) : base(settings)
        {
            _repairFunciont = new(settings.RepairSettings, settings.Controler);
        }


        public override void SelectTask(GameObject obs)
        {
            obs.transform.position = _controler._base.transform.position + (Vector3)(Random.insideUnitCircle * 2);
            //member.CurrentPosition = transform.position + (Vector3)(Random.insideUnitCircle * 2);
            _repairFunciont.StartRepairs(obs);
        }

        [System.Serializable]
        public class Settings : BuildSettings
        {
            public RepairFunc.Settings RepairSettings;
        }
    }
}