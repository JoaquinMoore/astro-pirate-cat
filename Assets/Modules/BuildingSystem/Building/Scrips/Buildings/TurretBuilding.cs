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
    }
}