using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden.States
{
    public class Idle : State<Context>
    {
        public Idle(Context controller) : base(controller) { }

        public override void Update() {
            if (_context.Input.HorizontalDirection() != 0 || _context.Input.VerticalDirection() != 0) {
                _context.SwitchState(new Walk(_context));
            }
        }
    }
}
