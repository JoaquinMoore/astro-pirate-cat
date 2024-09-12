using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden.Attack.States
{
    public class AttackState : State<AttackContext>
    {
        public AttackState(AttackContext context) : base(context) { }

        public override void Update()
        {
            var distance = _context.CalculateDistanceToTarget();
            if (distance <= _context.MeleeWeaponDistance)
            {
                _context.SelectMeleeWeapon();
                _context.Attack();
            }
            else if (distance <= _context.RangeWeaponDistance)
            {
                _context.SelectRangeWeapon();
                _context.Attack();
            }
            else
                _context.SwitchState(new IdleAttackState(_context));
        }
    }
}
