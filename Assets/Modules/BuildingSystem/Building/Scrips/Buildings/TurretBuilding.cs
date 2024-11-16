using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BuildSystem
{
    public class TurretBuilding : BuildingBase//, IBuildInteractuable, IInteractable
    {
        [SerializeField] private TurretBuildingModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;

        public event UnityAction OnCompleted;
        
        public void Start()
        {
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new TurretBuildingModel(_ModelSettings));
        }
        public void Focus()
        {
            _controler.Focus();
        }

        public void Unfocus()
        {
            _controler.Unfocus();
        }

        //public Task Interact(CrewMember crewMember)
        //{
        //    return _task;
        //}


        public void SelectTask(GameObject obs)
        {
            _controler.SelectTask(obs);
        }
        

        private void OnDrawGizmos()
        {
            float angle = _ModelSettings.TurretSettings.LookingAngle;

            float fow = _ModelSettings.TurretSettings.FieldOfView;

            Vector2 V2angle = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

            Gizmos.DrawLine(transform.position, (V2angle * _ModelSettings.TurretSettings.ViewRange) + (Vector2)transform.position);

            Vector2 ang1 = new Vector2(Mathf.Sin(((-fow / 2.0f) + angle) * Mathf.Deg2Rad), Mathf.Cos(((-fow / 2.0f) + angle) * Mathf.Deg2Rad));
            Vector2 ang2 = new Vector2(Mathf.Sin(((fow / 2.0f) + angle) * Mathf.Deg2Rad), Mathf.Cos(((fow / 2.0f) + angle) * Mathf.Deg2Rad));
            //
            //
            Gizmos.DrawLine(transform.position, (ang1 * _ModelSettings.TurretSettings.ViewRange) + (Vector2)transform.position);
            Gizmos.DrawLine(transform.position, (ang2 * _ModelSettings.TurretSettings.ViewRange) + (Vector2)transform.position);
        }

    }
}