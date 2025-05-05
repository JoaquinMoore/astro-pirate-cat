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
    [SerializeReference] public BlackboardVariable<float> CurrentSpeed;

    private SteeringMovement _movementService;

    private Vector2 TargetPosition => Target.Value.transform.position;

    protected override Status OnStart()
    {
        _movementService = new SteeringMovement(Data, Self.Value.transform);
        return base.OnStart();
    }

    protected override Status OnUpdate()
    {
        var velocity = (Vector3)_movementService
            .Seek(TargetPosition)
            .Flee(TargetPosition)
            .Separation()
            .GetNextPosition();
        Self.Value.transform.position += velocity * Time.deltaTime;

        CurrentSpeed.Value = velocity.magnitude;
        return Status.Running;
    }
}