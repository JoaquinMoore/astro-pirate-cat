using _UTILITY;
using UnityEngine;

namespace Physics.Movement
{
    public class MovementService : ApeBehaviour
    {
        private enum Method
        {
            GoTo,
            Seek
        }

        private Vector2 _currentDestiny;
        private Method _currentMethod;
        private readonly SteeringMovement _steeringMovement;
        private readonly Transform _transform;

        public MovementService(Transform transform, SteeringMovementDataSO data)
        {
            _transform = transform;
            _steeringMovement = new SteeringMovement(data, transform);
        }

        protected override void Update()
        {
            switch (_currentMethod)
            {
                case Method.GoTo:
                    _steeringMovement
                        .AddMoveForce(_currentDestiny)
                        .AddSeparationForce()
                        .GetNextPosition();
                    break;
                case Method.Seek:
                    _steeringMovement
                        .AddSeekForce(_currentDestiny)
                        .AddSeparationForce()
                        .AddFleeForce(_currentDestiny)
                        .GetNextPosition();
                    break;
            }

            _transform.position = _steeringMovement.GetNextPosition();
        }

        public void ApproachTo(Vector2 destiny)
        {
            _currentDestiny = destiny;
            _currentMethod = Method.Seek;
        }

        public void GoTo(Vector2 position)
        {
            _currentDestiny = position;
            _currentMethod = Method.GoTo;
        }
    }
}