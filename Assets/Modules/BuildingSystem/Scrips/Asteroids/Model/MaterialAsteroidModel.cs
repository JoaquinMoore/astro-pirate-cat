using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class MaterialAsteroidModel : BuildingModel
    {
        private AsteroidMinableFunc _mine;

        public MaterialAsteroidModel(Settings settings) : base(settings)
        {
            _mine = new(settings.MineSettings, this);
        }

        public override void SelectTask(GameObject obs)
        {
            _mine.MineFunc();

        }

        public override void Death()
        {
            Object.Destroy(_controler._base.gameObject);
        }


        [System.Serializable]
        public class Settings : BuildSettings
        {
            public AsteroidMinableFunc.Settings MineSettings;

        }

    }
}