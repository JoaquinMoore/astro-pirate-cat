using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Stunned", story: "[stunned]", category: "Conditions", id: "8f938ba6cb8f8f498af44b40d616cdd7")]
public partial class StunnedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> Stunned;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
