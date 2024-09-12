using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class MineBuildingModel : BuildingModel
    {
        MineFunc _mineFuncion;

        public MineBuildingModel(Settings settings) : base(settings)
        {
            _mineFuncion = new(settings.MineSettings, _controler);
        }
        public override void VirtualUpdate()
        {
            _mineFuncion.CheckNearby();
        }
        public override void Death()
        {
            Object.Destroy(_controler._base.gameObject);

        }

        [System.Serializable]
        public class Settings : BuildSettings
        {
            public MineFunc.Settings MineSettings;
        }

    }
}