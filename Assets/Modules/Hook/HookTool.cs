using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class HookTool : MonoBehaviour
{
    private const float MIN_APPROACH_DISTANCE = 0.01f;

    [Header("Approach anchor")]
    [SerializeField, Min(0)]
    private float _minDistance;
    [SerializeField, Min(0)]
    private float _approachSpeed;

    private DistanceJoint2D _joint;

    private void Awake()
    {
        _joint = GetComponent<DistanceJoint2D>();
        _joint.enabled = false;
        _joint.autoConfigureDistance = true;
    }

    public void Grab(Collider2D collider, GameObject hook)
    {
        if (collider.TryGetComponent<HookAnchor>(out var anchor))
        {
            hook.transform.position = collider.transform.position;
            _joint.connectedAnchor = hook.transform.position;
            _joint.enabled = true;

            if (anchor.typeOfAnchor == HookAnchor.AnchorType.Approach)
            {
                StopAllCoroutines();
                StartCoroutine(Approach());
            }
        }
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