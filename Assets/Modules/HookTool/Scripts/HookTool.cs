using System.Collections;
using UnityEngine;

namespace Core.HookTool
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class HookTool : MonoBehaviour
    {
        private const float MIN_APPROACH_DISTANCE = 0.01f;

        public float MinDistance => _minDistance;

        [Header("Approach anchor")]
        [SerializeField, Min(0), Tooltip("La distancia minima a la que se puede acercar el gancho.")]
        private float _minDistance;
        [SerializeField, Min(0), Tooltip("La velocidad con la que se acerca el gancho.")]
        private float _approachSpeed;

        private DistanceJoint2D _joint;

        /// <summary>
        /// Engancha el objeto a un anchor, y dependiendo del anchor va a tener distintos comportamientos.
        /// </summary>
        /// <param name="anchor">El anchor al que se engancha.</param>
        /// <param name="hook">El objeto que srivió como gancho.</param>
        public void Grab(HookAnchor anchor, GameObject hook = null)
        {
            if (hook == null) hook = anchor.gameObject;

            hook.transform.position = anchor.transform.position;
            _joint.connectedAnchor = hook.transform.position;
            _joint.enabled = true;
            StopAllCoroutines();

            if (anchor.typeOfAnchor == HookAnchor.AnchorType.Approach) StartCoroutine(Approach());
        }

        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
            _joint.autoConfigureDistance = true;
        }

        private IEnumerator Approach()
        {
            while (_joint.distance >= Mathf.Max(_minDistance, MIN_APPROACH_DISTANCE))
            {
                _joint.distance -= _approachSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}