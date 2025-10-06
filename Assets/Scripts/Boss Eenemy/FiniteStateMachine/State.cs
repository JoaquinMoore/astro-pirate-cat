namespace NPC.FSM
{
    public abstract class State
    {
        public virtual void Enter() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void Exit() { }
    }

    public abstract class State<T> : State
    {
        protected T context;
        protected FiniteStateMachine fsm;

        public State(T context, FiniteStateMachine fsm)
        {
            this.context = context;
            this.fsm = fsm;
        }
    }

    [System.Serializable]

    public class StateSettings
    {

    }
}
