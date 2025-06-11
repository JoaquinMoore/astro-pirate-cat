using Npc;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckTask", story: "Do the task of the [NPC]", category: "Action", id: "6903ce2c06880d6819e51c9e0864d20e")]
public partial class CheckTaskAction : Action
{
    [SerializeReference] public BlackboardVariable<NPCController> NPC;

    protected override Status OnUpdate()
    {
        NPC.Value.CheckTask();
        return Status.Running;
    }
}