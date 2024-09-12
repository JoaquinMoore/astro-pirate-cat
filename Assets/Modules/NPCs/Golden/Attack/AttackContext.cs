using UnityEngine;
using Assets.Modules.Util.StateMachine;
using Assets.Modules.NPCs.Golden.Attack.Interfaces;
using Assets.Modules.NPCs.Golden.Attack.States;

namespace Assets.Modules.NPCs.Golden.Attack
{
    public class AttackContext : StateContext<AttackContext>
    {
        [field: SerializeField] public float RangeWeaponDistance { get; private set; }
        [field: SerializeField] public float MeleeWeaponDistance { get; private set; }
        private IWeaponsController _weaponsController;
        private IDistanceCalculator _distanceCalculator;
        private ITargetDetector _targetDetector;
        private GameObject _target;

        public void Start()
        {
            _distanceCalculator = GetComponent<IDistanceCalculator>();
            _targetDetector = GetComponent<ITargetDetector>();
            _weaponsController = GetComponent<IWeaponsController>();
            Initialize(new IdleAttackState(this));
        }

        public void SelectRangeWeapon() => _weaponsController.SelectRangeWeapon();
        public void SelectMeleeWeapon() => _weaponsController.SelectMeleeWeapon();
        public float CalculateDistanceToTarget()
        {
            _target = _targetDetector.DetectTarget();
            return _distanceCalculator.CalculateDistanceToTarget(_target);
        }

        public void Attack()
        {
            _weaponsController.SetTarget(_target);
            _weaponsController.Attack();
        }
    }
}
