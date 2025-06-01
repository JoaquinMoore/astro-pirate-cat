using System.Collections.Generic;
using Npc.Tasks;
using Physics.Movement;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCFacade : MonoBehaviour, IInteractor
    {
        private Enumerators.Team _team;
        private MovementService _movement;
        private readonly Queue<BaseNPCTask> _tasks = new();

        private MovementService Movement => _movement ??= ServiceLocator.For(this).Get<MovementService>();

        private void Start()
        {
            // ServiceLocator.For(this)
            //     .Get(out _movement);
        }

        public void AddTask(BaseNPCTask newTask)
        {
            print(newTask.Log());
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