using System;
using System.Linq;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace AstroCat.BehaviourTree
{
    [Serializable]
    [GeneratePropertyBag]
    [NodeDescription(
        "Detection Radius 2D",
        "Use OverlapCircleAll to return all the detections in a radius",
        "Check detections in [radius] from [self]",
        category: "Action",
        id: "9ccbe37b45d39ac7b7a46e7b19721726")]
    public class DetectionRadius2DAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Radius;
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<string> Tag;
        [SerializeReference] public BlackboardVariable<GameObject> Detection;

        protected override Status OnUpdate()
        {
            Detection.Value = Physics2D.OverlapCircleAll(Self.Value.transform.position, Radius.Value)
                .Where(d => d.CompareTag(Tag))
                .OrderBy(c => Vector2.Distance(c.transform.position, Self.Value.transform.position))
                .FirstOrDefault()?.gameObject;
            return Detection != null ? Status.Success : Status.Failure;
        }
    }
}