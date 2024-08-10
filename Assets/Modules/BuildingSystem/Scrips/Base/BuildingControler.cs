using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class BuildingControler
    {
        public BuildingBase _base { get; private set; }
        public BuildingVisual _visual { get; private set; }
        public BuildingModel _model { get; private set; }

        public void SetUp(BuildingVisual visual, BuildingModel model, BuildingBase based)
        {
            _visual = visual;
            _model = model;
            _base = based;
        }

        public void VirtualUpdate()
        {
            _model.VirtualUpdate();
        }



        public void Focus()
        {
            _visual.Focus();
            //foreach (var item in _renderes)
            //    item.color = _task.IsBlocked ? Color.yellow : _focusColor.Color;
        }

        public void Unfocus()
        {
            _visual.Unfocus();
            //foreach (var item in _renderes)
            //    item.color = Color.white;
        }

        //public Task Interact(CrewMember crewMember)
        //{
        //    return _task;
        //}


        public void SelectTask(GameObject obs)
        {
            _model.SelectTask(obs);
        }



        public void Damage()
        {
            _model.Damage();
        }
    }
}