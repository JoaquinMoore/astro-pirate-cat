namespace Assets.Modules.Util.StateMachine
{
    public abstract class State<ContextType>
    {
        protected ContextType _context;
        public State(ContextType context) => _context = context;

        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void OnExit() { }
    }
}
