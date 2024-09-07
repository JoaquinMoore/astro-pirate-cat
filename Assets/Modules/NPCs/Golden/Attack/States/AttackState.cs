using Assets.Modules.Util.StateMachine;
using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Attack.States
{
    public class AttackState : State<Context>
    {
        public AttackState(Context context) : base(context) { }

        public override void OnEnter() => Debug.Log("AttackState");

        public override void Update()
        {
            var distance = _context.CalculateDistanceToTarget();
            if (distance <= _context.MeleeWeaponDistance)
            {
                _context.SelectMeleeWeapon();
                _context.SetSpriteColor(Color.red);
            }
            else if (distance <= _context.RangeWeaponDistance)
            {
                _context.SelectRangeWeapon();
                _context.SetSpriteColor(Color.blue);
            }
            else
                _context.SwitchState(new IdleAttackState(_context));
        }
    }
}
