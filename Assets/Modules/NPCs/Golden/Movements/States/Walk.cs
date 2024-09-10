using Assets.Modules.Util.StateMachine;
using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Movements.States
{
    public class Walk : State<MovementContext>
    {
        public Walk(MovementContext controller) : base(controller) { }

        public override void Update()
        {
            var hDirection = _context.Input.HorizontalDirection();
            var vDirection = _context.Input.VerticalDirection();
            _context.Move(new Vector2(hDirection, vDirection));
            if (hDirection == 0 && vDirection == 0) {
                _context.SwitchState(new Idle(_context));
            }
        }
    }
}
