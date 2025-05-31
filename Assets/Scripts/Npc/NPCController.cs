using Physics.Movement;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;

namespace AstroCat.NPC
{
    public class NPCController : MonoBehaviour
    {
        private BehaviorGraphAgent _behaviorGraph;
        private ITargetable _defaultTarget;
        private MovementService _movement;
        private BlackboardVariable<GameObject> _targetVariable;

        private GameObject Target
        {
            get => _targetVariable.Value;
            set => _targetVariable.Value = value;
        }

        private void Start()
        {
            ServiceLocator.For(this)
                .Get(out _movement)
                .Get(out _behaviorGraph);
            _behaviorGraph.BlackboardReference.GetVariable("Target", out _targetVariable);
        }

        public void Move(Vector2 direction)
        {
        }

        public void Speak(string mensaje)
        {
            Debug.Log(mensaje);
        }
    }
}