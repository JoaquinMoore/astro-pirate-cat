using System;
using _UTILITY;
using UnityEngine;

namespace Physics.Movement
{
    public class MovementService : ApeBehaviour
    {
        public Vector2 CurrentDestiny => _currentDestiny;

        private Vector2 _currentDestiny;
        private Action _calculateSteering;
        private readonly SteeringMovement _steeringMovement;
        private readonly Transform _transform;
        private bool _moving;

        public MovementService(Transform transform, SteeringMovement.Data data)
        {
            _transform = transform;
            _steeringMovement = new SteeringMovement(data, transform);
        }

        public void GoTo(Vector2 position)
        {
            _moving = true;
            _currentDestiny = position;
            _steeringMovement
                .PreciseArrival(position);
        }

        protected override void Update()
        {
            // _calculateSteering();
            if (!_moving)
            {
                _steeringMovement.AddStopForce();
            }
            _transform.position = _steeringMovement.GetNextPosition();
            _moving = false;
        }

        // private void DoApproachTo()
        // {
        //     _steeringMovement
        //         .AddArrivalForce(_currentDestiny);
        // }
        //
        // public void ApproachTo(Vector2 destiny)
        // {
        //     _currentDestiny = destiny;
        //     _calculateSteering = DoApproachTo;
        // }
        //
        // public void GoTo(Vector2 position)
        // {
        //     _currentDestiny = position;
        //     _calculateSteering = DoGoTo;
        // }
    }
}