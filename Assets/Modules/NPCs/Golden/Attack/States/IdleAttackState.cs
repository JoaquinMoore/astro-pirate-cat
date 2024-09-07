using Assets.Modules.Util.StateMachine;
using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Attack.States
{
    public class IdleAttackState : State<Context>
    {
        public IdleAttackState(Context context) : base(context) { }

        public override void OnEnter() => _context.SetSpriteColor(Color.white);
            
        public override void Update()
        {
            var distance = _context.CalculateDistanceToTarget();
            if (distance <= _context.RangeWeaponDistance)
                _context.SwitchState(new AttackState(_context));
        }
    }
}
