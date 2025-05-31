using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityServiceLocator;

namespace Physics.Movement
{
    public class MovementService : MonoBehaviour
    {
        private SteeringMovement _steeringMovement;
        private SteeringMovementDataSO _steeringMovementData;

        private void Awake()
        {
            ServiceLocator.For(this).Get(out _steeringMovementData);
        }

        private void Start()
        {
            _steeringMovement = new SteeringMovement(_steeringMovementData, transform);
        }

        private void FixedUpdate()
        {
            _steeringMovement.AddSeekForce(Mouse.current.WorldPosition());
            transform.position = _steeringMovement.GetNextPosition();
        }
    }
}