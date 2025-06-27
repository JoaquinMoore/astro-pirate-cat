using System;
using Physics.Movement;
using TaskSystem;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCController : MonoBehaviour
    {
        public Data NPCData => _data;

        private Data _data;
        private Enumerators.Team _team;
        private MovementService _movement;
        private TasksController<NPCController> _tasksController;
        private MovementService Movement => _movement ??= ServiceLocator.For(gameObject).Get<MovementService>();
        private TasksController<NPCController> TasksController => _tasksController ??= ServiceLocator.For(gameObject).Get<TasksController<NPCController>>();

        public Barco barco;

        private void Start()
        {
            _movement = Movement;
            _tasksController = TasksController;
            ServiceLocator.For(gameObject).TryGet(out _data);
            GoTo(Vector2.zero);
        }

        public void SetDefaultTask(ITask<NPCController> baseTask) => TasksController.DefaultTask = baseTask;

        public void CheckTask() => TasksController.CheckTask();

        public void GoTo(Vector2 position) => Movement.GoTo(position);

        public void ApproachTo(Vector2 destiny) => Movement.ApproachTo(destiny);

        [Serializable]
        public class Data
        {
            public SteeringMovementDataSO _movementData;
            public float ViewDistance;
        }
    }
}