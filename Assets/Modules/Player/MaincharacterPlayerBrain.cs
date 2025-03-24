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
        _swapWeapon = InputSystem.actions.FindAction("Swap");
    }

    private void Update()
    {
        _controller.Attack(_attackAction.IsPressed());

        if (_hookAction.WasPressedThisFrame() && !_controller.Hooking())
        {
            var target = RadiusDetector.CircleDetector(Mouse.current.WorldPosition(), 2).OrderBy(c => Vector2.Distance(Mouse.current.WorldPosition(), c.transform.position)).FirstOrDefault();
            if (target != null)
                _controller.Hook(target.transform.position);
            else
                _controller.Hook(Mouse.current.WorldPosition());
        }
        else if (_hookAction.WasPressedThisFrame() && _controller.Hooking())
        {
            _controller.UnHook();
        }
        
        _controller.AimTo(Mouse.current.WorldPosition());

        if (_swapWeapon.WasPressedThisFrame())
        {
            _controller.SwapPrimWeapon();
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            _controller.ScrollWeapons(Input.mouseScrollDelta.y);
        }


    }

    private IMaincharacterController _controller;
    private InputAction _attackAction;
    private InputAction _hookAction;
    private InputAction _swapWeapon;
}
