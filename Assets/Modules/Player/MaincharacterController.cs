using System;
using Extensions;
using HealthSystem;
using HookToolSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WeaponSystem;

public class MaincharacterController : MonoBehaviour
{
    private InputAction _onAttackAction;
    private InputAction _onHookAction;
    private SpriteRenderer _spriteRenderer;

    public void Hook(Vector3 target)
    {
        _hookTool.Hooking(target);
    }
    public void UnHook()
    {
        _hookTool.Ungrab();
    }

    public bool Hooking()
    {
        return _hookTool.Hooking();
    }

    public void Attack(bool value)
    {
        if (value)
        {
            _weaponcontroller.PrimaryFireDown();
        }
        else
        {
            _weaponcontroller.PrimaryFireUp();
        }
    }

    public void AimTo(Vector2 target)
    {
        _weaponcontroller.MouseAim(target);
    }

    public void SwapPrimWeapon()
    {
        _weaponcontroller.SwapPrimaryWeapon();
        _softmaxSpeed = _weaponcontroller.GetSoftSpeedCap();
    }

    public void ScrollWeapons(float rot)
    {
        _weaponcontroller.ChangeSecondaryWeapon(rot);
    }

    protected void Impulse(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void LimitVelocity(float Hlimit, float Slimit)
    {
        if (_rigidBody.linearVelocity.magnitude > Slimit)
        {
            _rigidBody.linearVelocity = Vector2.Lerp(_rigidBody.linearVelocity, _rigidBody.linearVelocity.normalized * Slimit, ((_rigidBody.linearVelocity.magnitude - Slimit) / 5) * Time.deltaTime);
        }

        _rigidBody.linearVelocity = Vector2.ClampMagnitude(_rigidBody.linearVelocity, Hlimit);
    }



    public void OnHit()
    {
        _anims.SetTrigger("Hit");
        _hpBar.value = _hp.CurrentHealth / (float)_hp.PublicMaxHealth;
    }

    public void OnHeal()
    {

    }

    public void OnDeath()
    {
        _anims.SetTrigger("Death");
        GameManager.Instance.FailState();
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _hookTool = GetComponent<HookTool>();
        _weaponcontroller = GetComponent<WeaponControler>();

        _hpBar = GetComponentInChildren<Slider>();
        _anims = GetComponent<Animator>();
        _hp = GetComponentInChildren<Health>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _weaponcontroller.OnImpulse += Impulse;
        _onAttackAction = InputSystem.actions.FindAction("Attack");
        _onHookAction = InputSystem.actions.FindAction("Hook");
    }

    private void OnDisable()
    {
        _weaponcontroller.OnImpulse -= Impulse;
    }

    private void Update()
    {
        if (_onAttackAction.IsPressed())
        {
            _weaponcontroller.PrimaryFireDown();
        }
        else if (_onAttackAction.WasReleasedThisFrame())
        {
            _weaponcontroller.PrimaryFireUp();
        }

        if (_onHookAction.WasPressedThisFrame())
        {
            _hookTool.Hooking(Mouse.current.WorldPosition());
        }

        var mustFlip = Mouse.current.WorldPosition().x > transform.position.x;
        _weaponcontroller.MouseAim(Mouse.current.WorldPosition(), mustFlip);
        _spriteRenderer.flipX = mustFlip;
    }

    private void FixedUpdate()
    {
        LimitVelocity(_maxSpeed, _softmaxSpeed);
    }

    private HookTool _hookTool;
    private Rigidbody2D _rigidBody;
    private WeaponControler _weaponcontroller;

    private Slider _hpBar;
    private Animator _anims;
    private Health _hp;


    [Header("Config")]
    [SerializeField]
    private float _maxSpeed = 10;
    [SerializeField]
    private float _softmaxSpeed = 5;
}