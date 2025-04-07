using UnityEngine;
using UnityEngine.Events;

namespace BuildSystem
{
    public class DefenceBuilding : BuildingBase //, IBuildInteractuable, IInteractable
    {
        [SerializeField] private DefenceBuildingModel.Settings _ModelSettings;
        [SerializeField] private BuildingVisual.VisualSettings _VisualSettings;

        public event UnityAction OnCompleted;

        public GameObject testobj;
        void Start()
        {
            _ModelSettings.Controler = _controler;
            SetUp(new BuildingVisual(_VisualSettings), new DefenceBuildingModel(_ModelSettings));
        }



        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Damage();
                Debug.Log(_controler._model._hpFuncion.CurrentHealth);
            }

            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    SelectTask(Instantiate(testobj));
            //}
            base.Update();
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



    }
}