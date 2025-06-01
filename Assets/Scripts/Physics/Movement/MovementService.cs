using _UTILITY;
using UnityEngine;

namespace Physics.Movement
{
    public class MovementService : ApeBehaviour
    {
        private readonly SteeringMovement _steeringMovement;
        private readonly Transform _transform;

        public MovementService(Transform transform, SteeringMovementDataSO data)
        {
            _transform = transform;
            _steeringMovement = new SteeringMovement(data, transform);
        }

        public void ApproachTo(Vector2 destiny)
        {
            _steeringMovement
                .AddSeekForce(destiny)
                .AddSeparationForce()
                .AddFleeForce(destiny);
            _transform.position = _steeringMovement.GetNextPosition();
        }

        public void GoTo(Vector2 position)
        {
            _steeringMovement
                .AddMoveForce(position)
                .AddSeparationForce();
            _transform.position = _steeringMovement.GetNextPosition();
        }
    }
}