using System;
using System.Collections.Generic;
using System.Linq;
using Npc;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectEnemy", story: "[Self] detect near enemy", category: "NPC", id: "1e831063a5cbf876a2d23a65be19c2a0")]
public partial class DetectEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<NPCController> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Detection;

    protected override Status OnUpdate()
    {
        var detections = Physics2D.OverlapCircleAll(Self.Value.transform.position, Self.Value.NPCData.ViewDistance);
        Collider2D detection = null;

        if (detections.Length > 0)
        {
            detection = detections
                .Where(c => Self.Value.NPCData.DetectionTags.Contains(c.gameObject.tag))
                .OrderBy(c => Vector2.Distance(c.transform.position, Self.Value.transform.position))
                .FirstOrDefault();
        }

        Detection.Value = detection?.gameObject;
        return detection ? Status.Success : Status.Failure;
    }
}