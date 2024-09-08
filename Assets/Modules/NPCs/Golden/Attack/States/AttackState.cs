using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden.Attack.States
{
    public class AttackState : State<Context>
    {
        public AttackState(Context context) : base(context) { }

        public override void Update()
        {
            var distance = _context.CalculateDistanceToTarget();
            if (distance <= _context.MeleeWeaponDistance)
                _context.SelectMeleeWeapon();
            else if (distance <= _context.RangeWeaponDistance)
                _context.SelectRangeWeapon();
            else
                _context.SwitchState(new IdleAttackState(_context));
        }
    }
}
