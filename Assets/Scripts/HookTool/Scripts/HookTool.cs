using System.Collections;
using System.Linq;
using UnityEngine;

namespace HookToolSystem
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class HookTool : MonoBehaviour
    {
        private const float MIN_APPROACH_DISTANCE = 0.01f;

        [Header("Config")] [SerializeField] private float _maxSpeed = 10;
        [SerializeField] private GameObject _hookHeadPrefRef;
        [SerializeField] private SpriteRenderer _visualhookHeadRef;
        [SerializeField] private SpriteRenderer _visualhookArmRef;
        [SerializeField] private LayerMask _hookabletargets;

        [Header("HookConfig")] [SerializeField]
        private float _hookSpeed = 1.5f;

        [SerializeField] private float _hookReturnSpeed = 2f;
        [SerializeField] private float _hookDetectionRadius = 0.5f;

        [Header("Approach anchor")] [SerializeField] [Min(0)]
        private float _minDistance;

        [SerializeField] [Min(0)] private float _approachSpeed;

        private GameObject _currentAnchor;

        private GameObject _hook;
        private bool _hooked;
        private GameObject _hookHeadPref;
        private DistanceJoint2D _joint;
        private LineRenderer _lineRef;

        private SpriteRenderer _spriteRef;

        public float MinDistance => _minDistance;

        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
            _joint.autoConfigureDistance = true;
        }

        private void Start()
        {
            _spriteRef = GetComponent<SpriteRenderer>();
            _hookHeadPref = Instantiate(_hookHeadPrefRef);
            _hookHeadPref.SetActive(false);
            _lineRef = GetComponentInChildren<LineRenderer>();
            _lineRef.enabled = false;
        }

        public void Update()
        {
            if (_hooked)
                VisualHooking();
        }


        public bool Hooking()
        {
            return _hookHeadPref.activeSelf;
        }


        public void Hooking(Vector3 target)
        {
            var mousePos = target;
            StopAllCoroutines();

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

            while (distance > 0.1f * speed)
            {
                distance = Vector3.Distance(hooktarget, _hookHeadPref.transform.position);
                yield return new WaitForSeconds(0.000000001f);
                _hookHeadPref.transform.position += _hookHeadPref.transform.right * Time.fixedDeltaTime * speed;
                var targets = Physics2D.OverlapCircleAll(_hookHeadPref.transform.position, 0.5f, _hookabletargets);
                if (targets.Any())
                {
                    var target = targets.OrderBy(c =>
                        Vector2.Distance(c.transform.position, _hookHeadPref.transform.position)).First();
                    Grab(target, _hookHeadPref);
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
            var Rdistance = Vector2.Distance(_visualhookHeadRef.transform.position, _hookHeadPref.transform.position);


            while (Rdistance > 0.5f)
            {
                Debug.Log(Rdistance);
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

            _hookHeadPref.transform.right = hookpos;
        }

        public void Grab(Collider2D collider, GameObject hook = null)
        {
            if (collider?.gameObject == _currentAnchor)
                return;

            if (collider && collider.TryGetComponent<HookAnchor>(out var anchor))
            {
                _hook = hook != null ? hook : collider.gameObject;
                _hook.transform.position = collider.transform.position;
                _joint.connectedAnchor = _hook.transform.position;
                _joint.enabled = true;
                _currentAnchor = collider.gameObject;

                if (anchor.typeOfAnchor == HookAnchor.AnchorType.Approach)
                {
                    StopAllCoroutines();
                    StartCoroutine(Approach());
                }
            }
            else
            {
                Ungrab();
            }
        }

        public void Ungrab()
        {
            StopAllCoroutines();
            _joint.enabled = false;
            _currentAnchor = null;
            _hooked = false;

            _lineRef.enabled = false;
            _hookHeadPref.SetActive(false);
            _visualhookHeadRef.gameObject.SetActive(true);
        }

        public void UnGrab()
        {
            _joint.connectedAnchor = default;
            _joint.enabled = false;
            StopAllCoroutines();
        }

        private IEnumerator Approach()
        {
            while (_joint.distance > Mathf.Max(_minDistance, MIN_APPROACH_DISTANCE))
            {
                _joint.distance -= _approachSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}