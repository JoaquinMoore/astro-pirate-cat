using Physics.Movement;
using Unity.Behavior;
using UnityEngine;

namespace AstroCat.NPC
{
    public class NPCController : MonoBehaviour
    {
        private BehaviorGraphAgent _behaviorGraph;
        private ITargetable _defaultTarget;
        private SteeringMovement _movement;
        private BlackboardVariable<GameObject> _targetVariable;

        private GameObject Target
        {
            get => _targetVariable.Value;
            set => _targetVariable.Value = value;
        }

        private void Awake()
        {
            _behaviorGraph = GetComponent<BehaviorGraphAgent>();
        }

        private void Start()
        {
            _behaviorGraph.BlackboardReference.GetVariable("Target", out _targetVariable);
        }
    }
}