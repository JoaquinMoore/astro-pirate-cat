using Npc;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ApproachTo", story: "Approach [self] to [target]", category: "NPC", id: "adcc185f518b161796940afb5fff194c")]
public partial class ApproachToAction : Action
{
    public enum MovementType
    {
        Go,
        Approach,
        ApproachToAttack
    }

    [SerializeReference] public BlackboardVariable<NPCController> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<MovementType> TypeMovement;

    protected override Status OnStart()
    {
        if (!Target.Value)
        {
            return Status.Failure;
        }

        Self.Value.MoveTo(Target.Value.transform.position);

        // switch (TypeMovement.Value)
        // {
        //     case MovementType.Go:
        //         Self.Value.GoTo(Target.Value.transform.position);
        //         break;
        //     case MovementType.Approach:
        //         Self.Value.ApproachTo(Target.Value.transform.position);
        //         break;
        //     case MovementType.ApproachToAttack:
        //         Self.Value.ApproachToAttack(Target.Value.transform.position);
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }

        return Status.Success;
    }
}