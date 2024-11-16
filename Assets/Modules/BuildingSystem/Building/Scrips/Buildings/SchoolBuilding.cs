using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuildSystem
{
    public class SchoolBuilding : BuildingBase
    {

        [SerializeField] private SchoolBuildingModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("1");
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new SchoolBuildingModel(_ModelSettings));
        }


    }
}