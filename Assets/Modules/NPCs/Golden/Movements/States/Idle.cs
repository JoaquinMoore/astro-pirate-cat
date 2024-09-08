using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden.Movements.States
{
    public class Idle : State<MovementContext>
    {
        public Idle(MovementContext controller) : base(controller) { }

        public override void Update() {
            if (_context.Input.HorizontalDirection() != 0 || _context.Input.VerticalDirection() != 0) {
                _context.SwitchState(new Walk(_context));
            }
        }
    }
}
