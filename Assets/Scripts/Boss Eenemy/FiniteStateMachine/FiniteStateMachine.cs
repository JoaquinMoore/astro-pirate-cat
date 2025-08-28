using System;
using System.Collections.Generic;

namespace NPC.FSM
{
    public class FiniteStateMachine
    {
        private readonly Dictionary<Type, State> _states = new();

        public State CurrentState { get; private set; }

        public void ChangeState<T>(Type newState, T context)
        {
            if (!newState.IsSubclassOf(typeof(State)))
                throw new ArgumentException("El tipo especificado no es un subtipo de State.", newState.Name);

            if (CurrentState?.GetType() == newState)
                return;

            CurrentState?.Exit();

            if (!_states.TryGetValue(newState, out var state))
            {
                state = Activator.CreateInstance(newState, context, this) as State;

                _states.Add(newState, state);
            }

            CurrentState = state;
            CurrentState.Enter();
        }
    }
}
