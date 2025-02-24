using HookToolSystem;
using WeaponSystem;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using HealthSystem;

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

    public void Hooking()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StopAllCoroutines();

        _lineRef.enabled = true;
        _hookHeadPref.SetActive(true);
        _visualhookHeadRef.gameObject.SetActive(false);
        _hookHeadPref.transform.position = new Vector3(_visualhookHeadRef.transform.position.x, _visualhookHeadRef.transform.position.y, 0);
        Vector2 hookpos = (Vector2)_visualhookHeadRef.transform.position - (Vector2)mousePos;
        _hookHeadPref.transform.right = -hookpos;
        StartCoroutine(Anim(mousePos, _hookSpeed));

    }

    private IEnumerator Anim(Vector3 hooktarget, float speed)
    {
        float distance = Vector2.Distance(hooktarget, _hookHeadPref.transform.position);

        while (distance > 0.001f * speed)
        {
            distance = Vector3.Distance(hooktarget, _hookHeadPref.transform.position) - 10;
            yield return new WaitForSeconds(0.00001f);
            _hookHeadPref.transform.position += _hookHeadPref.transform.right * Time.fixedDeltaTime * speed;

            var targets = Physics2D.OverlapCircleAll(_hookHeadPref.transform.position, 0.5f, _hookabletargets);
            if (targets.Any())
            {
                var target = targets.OrderBy(c => Vector2.Distance(c.transform.position, _hookHeadPref.transform.position)).First();
                _hookTool.Grab(target, _hookHeadPref);
                _hooked = true;
                yield break;
            }



            _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
            _lineRef.SetPosition(1, _hookHeadPref.transform.position);
        }



        StartCoroutine(returnhook());
        yield return null;
    }

    public IEnumerator returnhook()
    {
        float Rdistance = Vector2.Distance(_visualhookHeadRef.transform.position, _hookHeadPref.transform.position);


        while (Rdistance > 0.5f)
        {
            Debug.Log(Rdistance);
            Rdistance = Vector3.Distance(_visualhookHeadRef.transform.position, _hookHeadPref.transform.position);
            yield return new WaitForSeconds(0.00001f);

            Vector2 hookpos = (Vector2)_visualhookHeadRef.transform.position - (Vector2)_hookHeadPref.transform.position;
            _hookHeadPref.transform.right = -hookpos;


            _hookHeadPref.transform.position += -_hookHeadPref.transform.right * Time.fixedDeltaTime * _hookReturnSpeed;

            _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
            _lineRef.SetPosition(1, _hookHeadPref.transform.position);
        }
        _hookHeadPref.SetActive(false);
        _lineRef.enabled = false;
        _visualhookHeadRef.gameObject.SetActive(true);
        yield return null;
    }


    #region Inputs
    public void Unhook()
    {
        StopAllCoroutines();
        StartCoroutine(returnhook());
        _hookTool.UnGrab();
        _hooked = false;

    }

    public void Shoot()
    {
        _weaponcontroller.PrimaryFireDown();
    }

    public void UnShoot()
    {
        _weaponcontroller.PrimaryFireUp();
    }

    public void SwapWeapons()
    {
        _weaponcontroller.SwapPrimaryWeapon();
    }

    public void Dash()
    {
        _anims.SetTrigger("Move");
    }

    public void MouseAim()
    {
        bool flip = false;

        Vector2 pos = (Vector2)Input.mousePosition - (Vector2)transform.position;

        if (pos.x > 500)
            flip = true;


        _weaponcontroller.MouseAim(Camera.main.ScreenToWorldPoint(Input.mousePosition), flip);
        FlipSprites(flip);

    }
    #endregion

    #region visual funcions
    public void FlipSprites(bool flip)
    {
        _spriteRef.flipX = flip;

        if (flip)
        {
            _visualhookArmRef.gameObject.transform.localPosition = _originalArmPos;
            if (!_hookHeadPref.activeSelf)
                _visualhookArmRef.gameObject.transform.localEulerAngles = new Vector3(0,0,0);
        }
        else
        {
            _visualhookArmRef.gameObject.transform.localPosition = new Vector3(_originalArmPos.x * -1, _originalArmPos.y, _originalArmPos.z);

            if (!_hookHeadPref.activeSelf)
                _visualhookArmRef.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }



    public void VisualHooking()
    {
        bool flip = false;
        Vector2 hookpos = (Vector2)_hookHeadPref.transform.position - (Vector2)transform.position;

        if (hookpos.x > 0)
            flip = true;

        hookpos = (Vector2)_hookHeadPref.transform.position - (Vector2)transform.position;
        _visualhookArmRef.transform.right = hookpos;
        if (!flip)
            _visualhookArmRef.gameObject.transform.localEulerAngles = new Vector3(180, 0, -_visualhookArmRef.gameObject.transform.eulerAngles.z);

        _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
        _lineRef.SetPosition(1, _hookHeadPref.transform.position);

        _hookHeadPref.transform.right = hookpos;

    }

    #endregion

    #region Damage


    public void OnHit()
    {
        _anims.SetTrigger("Hit");
        _hpBar.value = (float)_hp.PublicCurrentHealth / (float)_hp.PublicMaxHealth;
        Debug.Log((float)_hp.PublicCurrentHealth / (float)_hp.PublicMaxHealth);
    }

    public void OnHeal()
    {

    }

    public void OnDeath()
    {
        _anims.SetTrigger("Death");
        //GameManager.Instance.FailState();
    }




    #endregion
    public void LimitVelocity(float Hlimit, float Slimit)
    {
        if (_rigidBody.velocity.magnitude > Slimit)
        {
            _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _rigidBody.velocity.normalized * Slimit, ((_rigidBody.velocity.magnitude - Slimit) / 5) * Time.deltaTime);
        }
        _debugvelociy = _rigidBody.velocity;
        _debugvelociymag = _rigidBody.velocity.magnitude;
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, Hlimit);
    }




    private void Update()
    {
        if (_hooked)
            VisualHooking();


        if (Input.GetMouseButtonDown(1) && _hookHeadPref.activeSelf == false)
        {
            Hooking();
        }
        else if(Input.GetMouseButtonDown(1) && _hookHeadPref.activeSelf == true)
        {
            Unhook();
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapWeapons();
        }


        MouseAim();
        LimitVelocity(_maxSpeed, _softmaxSpeed);
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
        _weaponcontroller = GetComponent<WeaponControler>();
        _rigidBody = GetComponent<Rigidbody2D>();

        _spriteRef = GetComponent<SpriteRenderer>();
        _hookHeadPref = Instantiate(_hookHeadPrefRef);
        _hookHeadPref.SetActive(false);
        _hpBar = GetComponentInChildren<Slider>();
        _anims = GetComponent<Animator>();
        _hp = GetComponentInChildren<Health>();
        _lineRef = GetComponentInChildren<LineRenderer>();
        _lineRef.enabled = false;

        _originalArmPos = _visualhookArmRef.transform.localPosition;
    }


    private void Start()
    {
        _weaponcontroller.OnImpulse += Impulse;
    }
    private HookTool _hookTool;
    private Rigidbody2D _rigidBody;
    private WeaponControler _weaponcontroller;
    private SpriteRenderer _spriteRef;
    private LineRenderer _lineRef;
    private Slider _hpBar;
    private Animator _anims;
    private Health _hp;

    private float _softmaxSpeed = 5;

    private GameObject _hookHeadPref;
    private Vector3 _originalArmPos;
    private bool _hooked;



    [Header("Config")]
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private GameObject _hookHeadPrefRef;
    [SerializeField] private SpriteRenderer _visualhookHeadRef;
    [SerializeField] private SpriteRenderer _visualhookArmRef;
    [SerializeField] private LayerMask _hookabletargets;

    [Header("HookConfig")]
    [SerializeField] private float _hookSpeed = 1.5f;
    [SerializeField] private float _hookReturnSpeed = 2f;
    [SerializeField] private float _hookDetectionRadius = 0.5f;


    [Header("debug")]
    [SerializeField] Vector2 _debugvelociy;
    [SerializeField] float _debugvelociymag;
}
