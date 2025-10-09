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
    BuildRemove,
    Tactical
}


public class MaincharacterController : MonoBehaviour
{
    private InputAction _onAttackAction;
    private InputAction _onHookAction;
    private InputAction _onSwapAction;
    private InputAction _onOpenBuildMenuAction;


    private InputAction _onEscaoe;
    private InputAction _onNewGame;
    private InputAction _onRestart;

    private InputAction _onPlace;
    private InputAction _onRemove;
    private InputAction _onCloseBuildMenuAction;

    private InputAction _onSelectRemove;
    private InputAction _onDeselectRemove;
    private InputAction _onCloseDestroyBuildMenuAction;
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
        _onSwapAction = InputSystem.actions.FindAction("Swap");
        _onOpenBuildMenuAction = InputSystem.actions.FindAction("OpenBuild");

        _onEscaoe = InputSystem.actions.FindAction("Escape");
        _onNewGame = InputSystem.actions.FindAction("NewGame");
        _onRestart = InputSystem.actions.FindAction("Restart");

        _onPlace = InputSystem.actions.FindAction("Place");
        _onRemove = InputSystem.actions.FindAction("Remove");
        _onCloseBuildMenuAction = InputSystem.actions.FindAction("CloseBuild");

        _onSelectRemove = InputSystem.actions.FindAction("RemovePlace");
        _onDeselectRemove = InputSystem.actions.FindAction("RemoveRemove");
        _onCloseDestroyBuildMenuAction = InputSystem.actions.FindAction("CloseRemoveBuild");
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
            case ControlScheme.Build:
                BuildMenuInputs();
                break;
            case ControlScheme.BuildRemove:
                BuildMenuDestroy();
                break;
        }
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

        if (_onSwapAction.WasPressedThisFrame())
        {
            _weaponcontroller.SwapPrimaryWeapon();
        }
        if (_onOpenBuildMenuAction.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.ScreenControls();
        }



        if (Mouse.current.scroll.y.value != 0)
        {
            _weaponcontroller.ChangeSecondaryWeapon(Mouse.current.scroll.y.value);
        }

        var mustFlip = PWorldPosition(Mouse.current).x > transform.position.x;
        _weaponcontroller.MouseAim(PWorldPosition(Mouse.current), mustFlip);
        _spriteRenderer.flipX = mustFlip;
    }

    public void FinishMenuInputs()
    {
        if (_onRestart.WasPressedThisFrame())
        {
            GameManager.Instance.FinishGameControl();
        }
        if (_onNewGame.WasPressedThisFrame())
        {
            GameManager.Instance.RestartGame();
        }
        if (_onEscaoe.WasPressedThisFrame())
        {
            GameManager.Instance.ReturnToMenu();
        }

    }

    public void BuildMenuInputs()
    {
        GameManager.Instance._buildManager.MousePosition(Mouse.current.position.value);
        GameManager.Instance._buildManager.CheckVisual();

        if (_onPlace.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.PlaceHolo();
        }

        if (Mouse.current.scroll.y.value != 0)
        {
            GameManager.Instance._buildManager.ScrollFunc(Mouse.current.scroll.y.value);
        }

        if (_onRemove.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.RemoveHolo();
        }
        if (_onCloseBuildMenuAction.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.ScreenControls();
        }
    }

    public void BuildMenuDestroy()
    {
        //GameManager.Instance._buildManager.MousePosition(Mouse.current.position.value);
        GameManager.Instance._buildManager.MouseDestroyPos(GameManager.Instance._buildManager.FindPlacedObjects(Mouse.current.position.value), Mouse.current.position.value);


        if (_onSelectRemove.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.PlaceDestroyHolo();
        }

        if (_onDeselectRemove.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.RemoveDestroyHolo();
        }
        if (_onCloseDestroyBuildMenuAction.WasPressedThisFrame())
        {
            GameManager.Instance._buildManager.ScreenControls();
        }
    }


    public void SwichControlScreme(ControlScheme scheme)
    {
        _scheme = scheme;
        InputSystem.LoadLayout(scheme.ToString());   
        _weaponcontroller.PrimaryFireUp();
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