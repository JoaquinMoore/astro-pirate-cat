using System;
using System.Collections.Generic;
using Physics.Movement;
using TasksSystem;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCController : MonoBehaviour
    {
        public Task DefaultTask { get; set; }

        public Barco barco;

        private Enumerators.Team _team;
        private MovementService _movement;
        private readonly Queue<Task> _tasks = new();
        private Task _currentTask;

        private MovementService Movement => _movement ??= ServiceLocator.For(this).Get<MovementService>();

        public void AddTask(params Task[] newBaseTasks)
        {
            foreach (var task in newBaseTasks)
            {
                _tasks.Enqueue(task);
            }
        }

        [ContextMenu("Do Task")]
        public void DoTask()
        {
            EnsureCurrentTaskAssigned();

            if (_currentTask == null)
            {
                Debug.Log("No tengo ni task default");
            }
            else
            {
                _currentTask.Execute();
            }

            return;

            void EnsureCurrentTaskAssigned()
            {
                // if (_currentTask != null && _currentTask.CurrentState != BaseTask.State.Finished) return;

                if (!_tasks.TryDequeue(out _currentTask))
                {
                    _currentTask = DefaultTask;
                }
            }
        }

        private void Update()
        {
            if (barco)
            {
                ApproachTo(barco.transform.position);
            }
        }

        public void GoTo(Vector2 position) => Movement.GoTo(position);

        public void ApproachTo(Vector2 destiny) => Movement.ApproachTo(destiny);
    }
}