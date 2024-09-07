using UnityEngine;

namespace Assets.Modules.NPCs.Golden.States
{
    public class Walk : State
    {
        public Walk(Context controller) : base(controller) { }

        public override void OnEnter() => Debug.Log("Walk.OnEnter");
        public override void Update()
        {
            var hDirection = _context.Input.HorizontalDirection();
            var vDirection = _context.Input.VerticalDirection();
            _context.Move(new Vector2(hDirection, vDirection));
            if (hDirection == 0 && vDirection == 0) {
                _context.SwitchState(new Idle(_context));
            }
        }
        public override void OnExit() => Debug.Log("Walk.OnExit");
    }
}
