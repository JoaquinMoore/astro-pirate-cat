using System;
using Physics.Movement;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace AstroCat.BehaviourTree
{
    [Serializable]
    [GeneratePropertyBag]
    [NodeDescription("MoveTo", story: "Moves [self] to [target]", category: "Action",
        id: "5ecc71a62c2bfde7990863dbb8bf4bda")]
    public class MoveToAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<SteeringMovementDataSO> Data;
        [SerializeReference] public BlackboardVariable<float> CurrentSpeed;

        private SteeringMovement _movementService;

        private Vector2 TargetPosition => Target.Value.transform.position;

        protected override Status OnStart()
        {
            _movementService = new SteeringMovement(Data.Value, Self.Value.transform);
            return base.OnStart();
        }

        protected override Status OnUpdate()
        {
            if (!Target.Value)
                return Status.Running;

            //==== CALCULATE THE VELOCITY ======//
            var velocity = (Vector3)_movementService
                .AddSeekForce(TargetPosition)
                .AddFleeForce(TargetPosition)
                .AddSeparationForce()
                .GetNextPosition();

            //==== MOVES THE TRANSFORM POSITION ======//
            Self.Value.transform.position += velocity * Time.deltaTime;

            //==== RETURN THE CURRENT SPEED ======//
            CurrentSpeed.Value = velocity.magnitude;
            return Status.Running;
        }
    }
}