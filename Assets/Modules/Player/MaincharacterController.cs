using HookToolSystem;
using WeaponSystem;
using System.Linq;
using System;
using UnityEngine;

public class MaincharacterController : MonoBehaviour, IMaincharacterController
{
    public void Impulse(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void Hook(Collider2D target, GameObject hookHead)
    {
        _hookTool.Grab(target, hookHead);
    }

    public void Shoot()
    {
        _weaponcontroller.PrimaryFireDown();
    }

    public void UnShoot()
    {
        _weaponcontroller.PrimaryFireUp();
    }


    public void MouseAim()
    {
        bool test = false;

        Vector2 pos = (Vector2)Input.mousePosition - (Vector2)transform.position;

        if (pos.x > 500)
            test = true;

        _weaponcontroller.MouseAim(Camera.main.ScreenToWorldPoint(Input.mousePosition), test);


    }

    public void LimitVelocity(float Hlimit, float Slimit)
    {
        Debug.Log(_rigidBody.velocity.normalized * Slimit);

        if (_rigidBody.velocity.magnitude > Slimit)
        {
            _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _rigidBody.velocity.normalized * Slimit, ((_rigidBody.velocity.magnitude - Slimit) / 5) * Time.deltaTime);
        }
        debugvelociy = _rigidBody.velocity;
        debugvelociymag = _rigidBody.velocity.magnitude;
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, Hlimit);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var targets = Physics2D.OverlapCircleAll(mousePos, 1f);

            if (targets.Any())
            {
                var target = targets.OrderBy(c => Vector2.Distance(c.transform.position, mousePos)).First();
                _hookTool.Grab(target, null);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UnShoot();
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            _weaponcontroller.ChangeSecondaryWeapon(Input.mouseScrollDelta.y);
            _softmaxSpeed = _weaponcontroller.GetSoftSpeedCap();
        }

        MouseAim();
        LimitVelocity(_maxSpeed, _softmaxSpeed);
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
        _weaponcontroller = GetComponent<WeaponControler>();
        _rigidBody = GetComponent<Rigidbody2D>();

    }


    private void Start()
    {
        _weaponcontroller.OnImpulse += Impulse;
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
