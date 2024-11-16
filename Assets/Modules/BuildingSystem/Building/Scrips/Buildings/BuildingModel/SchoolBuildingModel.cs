using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class SchoolBuildingModel : BuildingModel
    {
        SchoolFunc _schoolfunc;
        MountingFunc _mountingfunc;
        RepairFunc _repairfunc;

        public SchoolBuildingModel(Settings settings) : base(settings)
        {
            _schoolfunc = new(settings.ShoolSettings, settings.Controler);
            _mountingfunc = new(settings.MountingSettings, settings.Controler);
            _repairfunc = new(settings.RepairSettings, settings.Controler);
        }


        public override void VirtualUpdate()
        {
            _schoolfunc.CheckSlots();
        }

        public override void SelectTask(GameObject obs)
        {
            if (_controler._base._Col.enabled)
            {
                _schoolfunc.GiveStuddent(_mountingfunc.SetCrew(obs));

                //task.Finish();
                return;
            }



            //member.CurrentPosition = transform.position + (Vector3)(Random.insideUnitCircle * 2);
            _repairfunc.StartRepairs(obs);
        }

        public override void ReadyToAnim()
        {
            _schoolfunc.Anims();
        }
        [System.Serializable]
        public class Settings : BuildSettings
        {
            public SchoolFunc.Settings ShoolSettings;
            public MountingFunc.Settings MountingSettings;
            public RepairFunc.Settings RepairSettings;
        }

    }
}