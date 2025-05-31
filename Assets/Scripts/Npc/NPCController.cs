using System.Collections.Generic;
using Extensions;
using Physics.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityServiceLocator;

namespace AstroCat.NPC
{
    public class NPCController : MonoBehaviour
    {
        private readonly Stack<INPCTask> _tasks = new();
        private ITargetable _currentTarget;

        private MovementService _movement;

        private void Start()
        {
            ServiceLocator.For(this)
                .Get(out _movement);
        }

        private void Update()
        {
            MoveTo(Mouse.current.WorldPosition());
        }

        public void SetNewTarget(ITargetable target)
        {
            _currentTarget = target;
        }

        public void MoveTo(Vector2 destiny)
        {
            _movement.GoTo(destiny);
        }
    }
}