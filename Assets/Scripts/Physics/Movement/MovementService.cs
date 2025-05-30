using UnityEngine;
using UnityServiceLocator;

namespace Physics.Movement
{
    public class MovementService : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.For(this).Get<SteeringMovement>();
        }
    }
}