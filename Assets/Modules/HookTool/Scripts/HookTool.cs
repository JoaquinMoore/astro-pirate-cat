using System.Collections;
using UnityEngine;

namespace HookToolSystem
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class HookTool : MonoBehaviour
    {
        private const float MIN_APPROACH_DISTANCE = 0.01f;

        public float MinDistance => _minDistance;

        [Header("Approach anchor")]
        [SerializeField, Min(0)]
        private float _minDistance;
        [SerializeField, Min(0)]
        private float _approachSpeed;

        private DistanceJoint2D _joint;

        public void Grab(Collider2D collider, GameObject hook = null)
        {
            if (collider && collider.TryGetComponent<HookAnchor>(out var anchor))
            {
                hook = hook != null ? hook : collider.gameObject;
                hook.transform.position = collider.transform.position;
                _joint.connectedAnchor = hook.transform.position;
                _joint.enabled = true;

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
            _joint.enabled = false;
        }

        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
            _joint.autoConfigureDistance = true;
        }

        private IEnumerator Approach()
        {
            while(_joint.distance > Mathf.Max(_minDistance, MIN_APPROACH_DISTANCE))
            {
                _joint.distance -= _approachSpeed * Time.deltaTime;
                yield return null;
            }
        }
    } 
}