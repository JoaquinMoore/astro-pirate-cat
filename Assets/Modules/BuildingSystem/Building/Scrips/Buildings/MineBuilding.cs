using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace BuildSystem
{
    public class MineBuilding : BuildingBase//, IBuildInteractuable
    {

        [SerializeField] private MineBuildingModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;


        void Start()
        {
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new MineBuildingModel(_ModelSettings));
        }



        public override void AnimDeathAction()
        {
            Destroy(gameObject);
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _ModelSettings.MineSettings.ExplosionRaduis);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _ModelSettings.MineSettings.DetectionRaduis);
        }

    }
}