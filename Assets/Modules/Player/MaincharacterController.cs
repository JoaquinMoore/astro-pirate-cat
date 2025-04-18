using HookToolSystem;
using WeaponSystem;
using UnityEngine;

using UnityEngine.UI;
using HealthSystem;
public class MaincharacterController : MonoBehaviour, IMaincharacterController
{
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
        _hpBar.value = (float)_hp.PublicCurrentHealth / (float)_hp.PublicMaxHealth;
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

        _weaponcontroller.OnImpulse += Impulse;
    }

    private void OnDisable()
    {
        _weaponcontroller.OnImpulse -= Impulse;
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
