using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BuildSystem
{
    public class HealingBuilding : BuildingBase//, IBuildInteractuable, IInteractable
    {

        [SerializeField] private HealingBuildingModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;

        public event UnityAction OnCompleted;

        void Start()
        {
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new HealingBuildingModel(_ModelSettings));
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