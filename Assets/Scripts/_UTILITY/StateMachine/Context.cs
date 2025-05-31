using UnityEngine;

namespace _UTILITY.StateMachine
{
    public abstract class StateContext<ContextType> : MonoBehaviour
    {
        private State<ContextType> _currentState;

        public void Initialize(State<ContextType> firstState) => _currentState = firstState;
        public void Update() => _currentState?.Update();
        public void SwitchState(State<ContextType> newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}
