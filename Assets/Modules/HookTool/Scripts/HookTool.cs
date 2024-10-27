using System.Collections;
using UnityEngine;

namespace HookToolSystem
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class HookTool : MonoBehaviour
    {
        public float MinDistance => Mathf.Max(_minDistance, MIN_APPROACH_DISTANCE);

        /// <summary>
        /// El personaje se engancha al collider.
        /// </summary>
        /// <param name="collider">El collider del objeto al que se quieren enganchar.</param>
        /// <param name="hook">El objeto que sirvio como gancho.</param>
        public void Grab(Collider2D collider, GameObject hook)
        {
            if (collider.TryGetComponent<HookAnchor>(out var anchor) && anchor != _lastHookedAnchor)
            {
                hook.transform.position = collider.transform.position;
                _joint.connectedAnchor = hook.transform.position;
                _joint.enabled = true;

                if (anchor.typeOfAnchor == HookAnchor.AnchorType.Approach)
                {
                    StopAllCoroutines();
                    StartCoroutine(Approach());
                }
                _lastHookedAnchor = anchor;
            }
        }

        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
            _joint.autoConfigureDistance = true;
        }

        private IEnumerator Approach()
        {
            float time = 0f;
            float initialDistance = _joint.distance;
            while(_joint.distance > MinDistance)
            {
                var curve = _approachMotion.Evaluate(time / _approachSpeed);
                _joint.distance = Mathf.Lerp(initialDistance, MinDistance, curve);
                time += Time.deltaTime;
                yield return null;
            }
        }

        [Header("Approach anchor")]
        [SerializeField, Min(0)]
        private float _minDistance;
        [SerializeField, Min(0)]
        private float _approachSpeed;
        [SerializeField]
        private AnimationCurve _approachMotion;

        private DistanceJoint2D _joint;
        private HookAnchor _lastHookedAnchor;

        private const float MIN_APPROACH_DISTANCE = 0.01f;
    }
}