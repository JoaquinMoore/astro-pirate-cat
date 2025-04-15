using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveTo", story: "Moves [self] to [target]", category: "Action", id: "5ecc71a62c2bfde7990863dbb8bf4bda")]
public partial class MoveToAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<SteeringMovementDataSO> Data;

    SteeringMovement _movementService;

    protected override Status OnStart()
    {
        _movementService = new(Data, Self.Value.transform);
        return base.OnStart();
    }

    protected override Status OnUpdate()
    {
        Self.Value.transform.position += (Vector3)_movementService
            .Seek(Target.Value.transform.position)
            .GetNextPosition() * Time.deltaTime;

        Debug.Log("Me estoy moviendo hacia " + Target.Value.name);
        return Status.Running;
    }
}

