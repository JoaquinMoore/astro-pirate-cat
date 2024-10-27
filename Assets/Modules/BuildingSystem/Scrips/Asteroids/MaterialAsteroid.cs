using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class MaterialAsteroid : BuildingBase
    {

        [SerializeField] private MaterialAsteroidModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;

        // Start is called before the first frame update
        void Start()
        {
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new MaterialAsteroidModel(_ModelSettings));
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.A))
            {
                _controler.SelectTask(null);
            }
        }

    }
}