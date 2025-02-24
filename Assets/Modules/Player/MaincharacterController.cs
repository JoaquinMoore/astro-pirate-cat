using HookToolSystem;
using WeaponSystem;
using UnityEngine;

public class MaincharacterController : MonoBehaviour, IMaincharacterController
{
    public void Hook(Collider2D target, GameObject hookHead = null)
    {
        _hookTool.Grab(target, hookHead);
    }

    public void Attack(bool value)
    {
        if (value)
            _weaponcontroller.PrimaryFireDown();
        else
            _weaponcontroller.PrimaryFireUp();
    }

    public void AimTo(Vector2 target)
    {
        _weaponcontroller.MouseAim(target);
    }

    protected void Impulse(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void LimitVelocity(float Hlimit, float Slimit)
    {
        if (_rigidBody.velocity.magnitude > Slimit)
        {
            _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _rigidBody.velocity.normalized * Slimit, ((_rigidBody.velocity.magnitude - Slimit) / 5) * Time.deltaTime);
        }
        debugvelociy = _rigidBody.velocity;
        debugvelociymag = _rigidBody.velocity.magnitude;
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, Hlimit);
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
        _weaponcontroller = GetComponent<WeaponControler>();
        _weaponcontroller.OnImpulse += Impulse;
        _rigidBody = GetComponent<Rigidbody2D>();
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

    private float _softmaxSpeed = 5;

    [Header("Config")]
    [SerializeField] private float _maxSpeed = 10;


    [Header("debug")]
    [SerializeField] Vector2 debugvelociy;
    [SerializeField] float debugvelociymag;
}
