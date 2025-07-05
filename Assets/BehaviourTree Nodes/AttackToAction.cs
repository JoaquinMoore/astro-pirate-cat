using Npc;
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTo", story: "[self] attack [to]", category: "NPC", id: "82c2f4be6cb86ec493517f30037c8d3a")]
public partial class AttackToAction : Action
{
    [SerializeReference] public BlackboardVariable<NPCController> Self;
    [SerializeReference] public BlackboardVariable<GameObject> To;

    protected override Status OnUpdate()
    {
        // Self.Value.AttackTo(To.Value);
        return Status.Success;
    }
}