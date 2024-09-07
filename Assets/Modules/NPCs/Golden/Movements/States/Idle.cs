using UnityEngine;

namespace Assets.Modules.NPCs.Golden.States
{
    public class Idle : State
    {
        public Idle(Context controller) : base(controller) { }

        public override void OnEnter() => Debug.Log("Idle Enter");
        public override void Update() {
            if (_context.Input.HorizontalDirection() != 0 || _context.Input.VerticalDirection() != 0) {
                _context.SwitchState(new Walk(_context));
            }
        }
        public override void OnExit() => Debug.Log("Idle Exit");
    }
}
