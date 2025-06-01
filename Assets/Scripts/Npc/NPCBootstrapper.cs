using Physics.Movement;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCBootstrapper : Bootstrapper
    {
        [SerializeField] private SteeringMovementDataSO _steeringMovementData;

        protected override void Bootstrap()
        {
            Register(GetComponent<BehaviorGraphAgent>());
            Register(new MovementService(transform, _steeringMovementData));
        }
    }
}