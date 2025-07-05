using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayAnimation", story: "Play this [clip]", category: "Action", id: "cf6f558467302e98ef28925b9a4124d8")]
public partial class PlayAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<AnimationClip> Clip;

    protected override Status OnStart()
    {
        return Status.Success;
    }
}