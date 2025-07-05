using System;
using Physics.Movement;
using TaskSystem;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;
using WeaponSystem;

namespace Npc
{
    public class NPCController : MonoBehaviour
    {
        public Data NPCData => _data;

        public GameObject Target
        {
            get {
                Agent.BlackboardReference.GetVariableValue("Enemy", out GameObject go);
                return go;
            }
            set => Agent.BlackboardReference.SetVariableValue("Enemy", value);
        }

        private BehaviorGraphAgent _agent;
        private Data _data;
        private Enumerators.Team _team;
        private MovementService _movement;
        private TasksController<NPCController> _tasksController;
        private WeaponControler _weaponController;

        private MovementService Movement => _movement ??= ServiceLocator.For(gameObject).Get<MovementService>();
        private TasksController<NPCController> TasksController => _tasksController ??= ServiceLocator.For(gameObject).Get<TasksController<NPCController>>();
        private BehaviorGraphAgent Agent => _agent ??= ServiceLocator.For(gameObject).Get<BehaviorGraphAgent>();

        private void Start()
        {
            ServiceLocator.For(gameObject).TryGet(out _data);
            _movement = Movement;
            _tasksController = TasksController;
            ServiceLocator.For(gameObject).TryGet(out _weaponController);
            Agent.BlackboardReference.SetVariableValue("AttackDistance", _data.AttackDistance);
        }

        private void Update()
        {
            if (Target)
            {
                _weaponController.MouseAim(Target.transform.position);
                HorizontalFlip(Target.transform.position.x > transform.position.x);
            }
            else
            {
                HorizontalFlip(_movement.CurrentDestiny.x > transform.position.x);
            }
        }

        public void SetDefaultTask(ITask<NPCController> baseTask) => TasksController.DefaultTask = baseTask;

        public void CheckTask() => TasksController.CheckTask();

        public void MoveTo(Vector2 position)
        {
            _movement.GoTo(position);
        }

        public void TriggerDown() => _weaponController.PrimaryFireDown();
        public void TriggerUp() => _weaponController.PrimaryFireUp();

        public void HorizontalFlip(bool flag)
        {
            transform.localScale = new Vector3(flag ? -1 : 1, transform.localScale.y, transform.localScale.z);
            Debug.Log(_movement.CurrentDestiny);
        }

        [Serializable]
        public class Data
        {
            public SteeringMovementDataSO _movementData;
            public float ViewDistance;
            public float AttackDistance;
            public string[] DetectionTags;
        }
    }
}