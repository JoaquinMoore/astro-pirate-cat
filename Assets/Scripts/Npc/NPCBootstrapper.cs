using Physics.Movement;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;

public class NPCBootstrapper : Bootstrapper
{
    [SerializeField] private SteeringMovementDataSO _steeringMovementData;

    protected override void Bootstrap()
    {
        Register(_steeringMovementData);
        Register(GetComponent<BehaviorGraphAgent>());
        Register(gameObject.AddComponent<MovementService>());
    }
}