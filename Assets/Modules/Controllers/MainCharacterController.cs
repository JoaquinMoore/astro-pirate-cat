using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    private float _anchorDetectionRadius;

    private MainCharacterFacade _facade;
    private InputAction _attackAction;
    private InputAction _hookAction;

    private void Awake()
    {
        _facade = GetComponent<MainCharacterFacade>();
        _attackAction = InputSystem.actions.FindAction("Attack");
        _hookAction = InputSystem.actions.FindAction("Hook");
    }

    private void Update()
    {
        if (_hookAction.WasPressedThisFrame())
        {
            //_facade.ShootHook();
            _facade.TryGrab(Mouse.current.WorldPosition(), _anchorDetectionRadius);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Mouse.current.WorldPosition(), _anchorDetectionRadius);
    }
}
