using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class TurretBuildingModel : BuildingModel
    {

        private RepairFunc RepairFuncion;
        private TurretFunc TurretFuncion;

        public TurretBuildingModel(Settings settings) : base(settings)
        {
            RepairFuncion = new(settings.RepairSettings, _controler);
            TurretFuncion = new(settings.TurretSettings, _controler);
        }

        public override void VirtualUpdate()
        {
            if (_controler._base._Col.enabled)
                TurretFuncion.FindEnemies();
        }

        //public void SelectTask(CrewMember member, Task task)
        public override void SelectTask(GameObject obs)
        {
            RepairFuncion.StartRepairs(obs);
        }


        [System.Serializable]
        public class Settings : BuildSettings
        {
            public RepairFunc.Settings RepairSettings;
            public TurretFunc.Settings TurretSettings;
        }

    }
}