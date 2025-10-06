using System.Collections;
using System.Linq;
using UnityEngine;

namespace HookToolSystem
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class HookTool : MonoBehaviour
    {
        private const float MIN_APPROACH_DISTANCE = 0.01f;

        [Header("Config")]
        [SerializeField] private GameObject _hookHeadPrefRef;
        [SerializeField] private SpriteRenderer _visualhookHeadRef;
        [SerializeField] private SpriteRenderer _visualhookArmRef;
        [SerializeField] private LayerMask _hookabletargets;

        [Header("HookConfig")] [SerializeField]
        private float _hookSpeed = 1.5f;

        [SerializeField] private float _hookReturnSpeed = 2f;
        [SerializeField] private float _hookDetectionRadius = 0.5f;
        [SerializeField] private float _hookLineStrenght = 200f;


        [Header("Approach anchor")]
        [SerializeField] [Min(0)] private float _minDistance;
        [SerializeField] [Min(0)] private float _maxApproachSpeed;
        [SerializeField] [Min(0)] private float _startingApproachSpeed;
        [SerializeField] [Min(0)] private float _ApproachRampupSpeed;
        [SerializeField] private AnimationCurve _approachSpeedCurve;

        [Header("Player Ref")]
        public Rigidbody2D _cont;

        [SerializeField] private GameObject _currentAnchor;
        [SerializeField] private GameObject _lastAnchor;

        private GameObject _hook;
        private bool _hooked;
        private bool _ungrabing;
        private GameObject _hookHeadPref;
        private DistanceJoint2D _joint;
        private LineRenderer _lineRef;

        private float _approachSpeed;

        public float MinDistance => _minDistance;
        private HookAnchor _anchor;
        Vector3 _inpulseDirection;
        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
            _joint.autoConfigureDistance = true;
        }

        private void Start()
        {
            _hookHeadPref = Instantiate(_hookHeadPrefRef);
            _hookHeadPref.SetActive(false);
            _lineRef = GetComponentInChildren<LineRenderer>();
            _lineRef.enabled = false;
        }

        public void Update()
        {
            if (_hooked)
                VisualHooking();
            if (_lastAnchor != null)
                CheckDistance();
        }

        public void CheckDistance()
        {

            if (Vector3.Distance(_lastAnchor.transform.position, transform.position) > 4)
            {
                _lastAnchor = null;
            }

        }


        public bool Hooking()
        {
            return _hookHeadPref.activeSelf;
        }


        public void Hooking(Vector3 target)
        {
            if (_hooked)
            {
                Ungrab();
                return;
            }


            var mousePos = target;
            StopAllCoroutines();
            _ungrabing = false;
            _lineRef.enabled = true;
            _hookHeadPref.SetActive(true);
            _visualhookHeadRef.gameObject.SetActive(false);
            _hookHeadPref.transform.position = new Vector3(_visualhookHeadRef.transform.position.x,
                _visualhookHeadRef.transform.position.y, 0);
            var hookpos = (Vector2)_visualhookHeadRef.transform.position - (Vector2)mousePos;
            _hookHeadPref.transform.right = -hookpos;
            StartCoroutine(Anim(mousePos, _hookSpeed));
        }

        private IEnumerator Anim(Vector3 hooktarget, float speed)
        {
            var distance = Vector2.Distance(hooktarget, _hookHeadPref.transform.position);

            while (distance > 0.1f)
            {

                distance = Vector3.Distance(hooktarget, _hookHeadPref.transform.position);
                yield return new WaitForSeconds(0.000000001f);
                _hookHeadPref.transform.position += _hookHeadPref.transform.right * Time.fixedDeltaTime * speed;
                var targets = Physics2D.OverlapCircleAll(_hookHeadPref.transform.position, 0.5f, _hookabletargets);
                if (targets.Any())
                {
                    var target = targets.OrderBy(c =>
                        Vector2.Distance(c.transform.position, _hookHeadPref.transform.position)).First();
                    if (target?.gameObject != _lastAnchor)
                    {
                        _joint.breakForce = _hookLineStrenght;
                        Grab(target, _hookHeadPref);
                        _hooked = true;
                        yield break;
                    }

                }


                _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
                _lineRef.SetPosition(1, _hookHeadPref.transform.position);
            }


            StartCoroutine(returnhook());
            yield return null;
        }

        public IEnumerator returnhook()
        {
            var Rdistance = Vector2.Distance(_visualhookHeadRef.transform.position, _hookHeadPref.transform.position);


            while (Rdistance > 0.5f)
            {
                //Debug.Log(Rdistance);
                Rdistance = Vector3.Distance(_visualhookHeadRef.transform.position, _hookHeadPref.transform.position);
                yield return new WaitForSeconds(0.00001f);

                var hookpos = (Vector2)_visualhookHeadRef.transform.position -
                              (Vector2)_hookHeadPref.transform.position;
                _hookHeadPref.transform.right = -hookpos;


                _hookHeadPref.transform.position +=
                    -_hookHeadPref.transform.right * Time.fixedDeltaTime * _hookReturnSpeed;

                _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
                _lineRef.SetPosition(1, _hookHeadPref.transform.position);
            }

            _hookHeadPref.SetActive(false);
            _lineRef.enabled = false;
            _visualhookHeadRef.gameObject.SetActive(true);
            yield return null;
        }

        public void VisualHooking()
        {
            var flip = false;
            var hookpos = (Vector2)_hookHeadPref.transform.position - (Vector2)transform.position;

            if (hookpos.x > 0)
                flip = true;


            hookpos = (Vector2)_hookHeadPref.transform.position - (Vector2)transform.position;
            _visualhookArmRef.transform.right = hookpos;
            if (!flip)
                _visualhookArmRef.gameObject.transform.localEulerAngles =
                    new Vector3(180, 0, -_visualhookArmRef.gameObject.transform.eulerAngles.z);

            _lineRef.SetPosition(0, _visualhookHeadRef.transform.position);
            _lineRef.SetPosition(1, _hookHeadPref.transform.position);

            if (_currentAnchor != null)
                _hookHeadPref.transform.position = _currentAnchor.transform.position;
            else if(!_ungrabing)
                Ungrab();

            _hookHeadPref.transform.right = hookpos;
        }

        public void Grab(Collider2D collider, GameObject hook = null)
        {
            if (collider?.gameObject == _currentAnchor || collider?.gameObject == _lastAnchor)
                return;

            if (collider && collider.TryGetComponent<HookAnchor>(out var anchor))
            {
                _hook = hook != null ? hook : collider.gameObject;
                _hook.transform.position = collider.transform.position;
                _joint.connectedAnchor = _hook.transform.position;
                _joint.enabled = true;
                _currentAnchor = collider.gameObject;
                anchor.OnHook.Invoke();
                _anchor = anchor;

                _inpulseDirection = _hook.transform.position - _cont.transform.position;

                if (anchor.typeOfAnchor == HookAnchor.AnchorType.Approach)
                {
                    StopAllCoroutines();
                    StartCoroutine(Approach());
                }
                if (anchor.typeOfAnchor == HookAnchor.AnchorType.Stun)
                {
                    StopAllCoroutines();
                    Ungrab();
                }

            }
            else
            {
                Ungrab();
            }
        }

        public void Ungrab()
        {
            _ungrabing = true;

            Debug.Log("ungrab");
            StopAllCoroutines();
            if (_currentAnchor != null)
                _lastAnchor = _currentAnchor;
            _joint.enabled = false;
            //_joint.breakForce = 0;
            _currentAnchor = null;
            _hooked = false;
            if (_anchor != null)
                _anchor.OnRealese.Invoke();

            _cont.angularVelocity = 0;
            _cont.linearVelocity = Vector2.zero;
            _cont.AddForce(_inpulseDirection * (_approachSpeed), ForceMode2D.Impulse);
            StartCoroutine(returnhook());
        }

        private void OnJointBreak2D(Joint2D joint)
        {
            if (_ungrabing)
                return;
            Debug.Log("break");

            StopAllCoroutines();
            _currentAnchor = null;
            _hooked = false;
            if (_anchor != null)
                _anchor.OnRealese.Invoke();

            StartCoroutine(returnhook());
        }
        private IEnumerator Approach()
        {
            float timer = 0;
            _approachSpeed = 0;
            while (_joint.distance > Mathf.Max(_minDistance, MIN_APPROACH_DISTANCE))
            {
                timer += Time.deltaTime;
                _approachSpeed = Mathf.Lerp(_startingApproachSpeed, _maxApproachSpeed, _approachSpeedCurve.Evaluate(timer/_ApproachRampupSpeed));
                _joint.distance -= _approachSpeed * Time.deltaTime;
                //Debug.Log(_cont.RigidBody.linearVelocity);
                yield return null;
            }

            Debug.Log("ungrab aproached");
            Ungrab();

        }
    }
}