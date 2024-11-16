using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class AsteroidMinableFunc
    {
        Materials _mat;
        BuildingModel _model;

        public AsteroidMinableFunc(Settings settings, BuildingModel model)
        {
            _mat = settings.Mat;
            _model = model;
        }

        public void MineFunc()
        {
            GameManager.Instance.AddMaterialAmount(_mat);
            _model.Death();
        }

        [System.Serializable]
        public class Settings
        {
            public Materials Mat;
        }
    }
}