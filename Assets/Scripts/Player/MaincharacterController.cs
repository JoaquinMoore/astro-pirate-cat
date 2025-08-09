using System;
using _EXTENSIONS;
using HealthSystem;
using HookToolSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WeaponSystem;


public enum ControlScheme
{
    Gameplay,
    FailWin,
    Menu,
    Build,
    Tactical
}


public class MaincharacterController : MonoBehaviour
{
    private InputAction _onAttackAction;
    private InputAction _onHookAction;

    private InputAction _onEscaoe;
    private InputAction _onNewGame;
    private InputAction _onRestart;

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

    public void SwapPrimWeapon()
    {
        _weaponcontroller.SwapPrimaryWeapon();
        _softmaxSpeed = _weaponcontroller.GetSoftSpeedCap();
    }

    public void ScrollWeapons(float rot)
    {
        _weaponcontroller.ChangeSecondaryWeapon(rot);
    }

    public void Impulse(Vector2 force)
    {
        //Debug.Log(force);
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void LimitVelocity(float Hlimit, float Slimit)
    {

        //Debug.Log(_rigidBody.linearVelocity);
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
        StopMovement();
    }

    public void DeathTrigger()
    {
        GameManager.Instance.FailState();
    }

    public void StopMovement()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.angularVelocity = 0;
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
        Cam = GetComponentInChildren<Camera>();

        _weaponcontroller.OnImpulse += Impulse;
        _onAttackAction = InputSystem.actions.FindAction("Attack");
        _onHookAction = InputSystem.actions.FindAction("Hook");

        _onEscaoe = InputSystem.actions.FindAction("Escape");
        _onNewGame = InputSystem.actions.FindAction("NewGame");
        _onRestart = InputSystem.actions.FindAction("Restart");
    }

    private void OnDisable()
    {
        _weaponcontroller.OnImpulse -= Impulse;
    }

    private void Update()
    {
        switch (_scheme)
        {
            case ControlScheme.Gameplay:
                 PlayerInputs();
                break;
            case ControlScheme.FailWin:
                FinishMenuInputs();
                break;
        }
        Debug.Log(_rigidBody.linearVelocity);
    }

    public void PlayerInputs()
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
            _hookTool.Hooking(PWorldPosition(Mouse.current));
        
        }

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    _hookTool.Hooking(PWorldPosition(Mouse.current));
        //
        //}


        var mustFlip = PWorldPosition(Mouse.current).x > transform.position.x;
        _weaponcontroller.MouseAim(PWorldPosition(Mouse.current), mustFlip);
        _spriteRenderer.flipX = mustFlip;
    }

    public void FinishMenuInputs()
    {
        if (_onRestart.IsPressed())
        {
            GameManager.Instance.FinishGameControl();
        }
        if (_onNewGame.IsPressed())
        {
            GameManager.Instance.RestartGame();
        }
        if (_onEscaoe.IsPressed())
        {
            GameManager.Instance.ReturnToMenu();
        }

    }

    public void MenuInputs()
    {

    }


    public void SwichControlScreme(ControlScheme scheme)
    {
        _scheme = scheme;

    }

    public Vector2 PWorldPosition(Mouse mouse)
    {
        return Cam.ScreenToWorldPoint(mouse.position.value);
    }


    private void FixedUpdate()
    {
        LimitVelocity(_maxSpeed, _softmaxSpeed);
    }

    private void OnDrawGizmos()
    {

    }




    private HookTool _hookTool;
    private Rigidbody2D _rigidBody;
    private WeaponControler _weaponcontroller;

    private Slider _hpBar;
    private Animator _anims;
    private Health _hp;

    private ControlScheme _scheme;

    public Rigidbody2D RigidBody => _rigidBody;

    [Header("Config")]
    [SerializeField]
    private float _maxSpeed = 10;
    [SerializeField]
    private float _softmaxSpeed = 5;
    public Camera Cam { get; private set; }
}