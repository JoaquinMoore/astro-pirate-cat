using System.Collections.Generic;
using Npc.Tasks.Interfaces;
using Physics.Movement;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCFacade : MonoBehaviour, IInteractor
    {
        private Enumerators.Team _team;
        private MovementService _movement;
        private readonly Queue<ITask> _tasks = new();

        private MovementService Movement => _movement ??= ServiceLocator.For(this).Get<MovementService>();

        public void AddTask(ITask newTask)
        {
            _tasks.Enqueue(newTask);
            _tasks.Dequeue().Execute(this);
        }

        public void GoTo(Vector2 position) => Movement.GoTo(position);

        public void ApproachTo(Vector2 destiny) => Movement.ApproachTo(destiny);

        public void Interact(Barco barco)
        {
            barco.Hurt(10);
        }
    }
}