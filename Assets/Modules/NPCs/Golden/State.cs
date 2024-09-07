namespace Assets.Modules.NPCs.Golden
{
    public abstract class State
    {
        protected Context _context;

        public State(Context context) => _context = context;

        public abstract void OnEnter();
        public abstract void Update();
        public abstract void OnExit();
    }
}
