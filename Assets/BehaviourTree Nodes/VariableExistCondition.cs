using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "VariableExist", story: "[Variable] exist", category: "Conditions", id: "8e5737581b34fd6e7a4bc1093406398b")]
public partial class VariableExistCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Variable;

    public override bool IsTrue()
    {
        return Variable.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}