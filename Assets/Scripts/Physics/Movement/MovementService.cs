using _UTILITY;
using UnityEngine;

namespace Physics.Movement
{
    public class MovementService : ApeBehaviour
    {
        private readonly SteeringMovement _steeringMovement;
        private readonly Transform _transform;
        private Vector2 _currentDestiny;

        public MovementService(Transform transform, SteeringMovementDataSO data)
        {
            _transform = transform;
            _steeringMovement = new SteeringMovement(data, transform);
            _currentDestiny = transform.position;
        }

        protected override void FixedUpdate()
        {
            DoMovement();
        }

        private void DoMovement()
        {
            _steeringMovement
                .AddSeekForce(_currentDestiny)
                .AddSeparationForce()
                .AddFleeForce(_currentDestiny);
            _transform.position = _steeringMovement.GetNextPosition();
        }

        public void GoTo(Vector2 destiny)
        {
            _currentDestiny = destiny;
        }
    }
}