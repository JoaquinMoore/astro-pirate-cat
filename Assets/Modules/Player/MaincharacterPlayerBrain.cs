using Extensions;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaincharacterPlayerBrain : MonoBehaviour
{
    private void Awake()
    {
        _controller = GetComponent<MaincharacterController>();
    }

    private void Start()
    {
        _attackAction = InputSystem.actions.FindAction("Attack");
        _hookAction = InputSystem.actions.FindAction("Hook");
    }

    private void Update()
    {
        _controller.Attack(_attackAction.IsPressed());

        if (_hookAction.WasPressedThisFrame())
        {
            var target = RadiusDetector.CircleDetector(Mouse.current.WorldPosition(), 2).OrderBy(c => Vector2.Distance(Mouse.current.WorldPosition(), c.transform.position)).FirstOrDefault();
            _controller.Hook(target);
        }

        _controller.AimTo(Mouse.current.WorldPosition());
    }

    private IMaincharacterController _controller;
    private InputAction _attackAction;
    private InputAction _hookAction;
}
