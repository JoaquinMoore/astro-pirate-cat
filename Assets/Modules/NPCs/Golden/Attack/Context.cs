using UnityEngine;
using Assets.Modules.Util.StateMachine;
using Assets.Modules.NPCs.Golden.Attack.Interfaces;
using Assets.Modules.NPCs.Golden.Attack.States;

namespace Assets.Modules.NPCs.Golden.Attack
{
    public class Context : StateContext<Context>
    {
        [SerializeField] public float RangeWeaponDistance;
        [SerializeField] public float MeleeWeaponDistance;
        private WeaponControler _weaponControler;
        private IDistanceCalculator _distanceCalculator;
        private ITargetDetector _targetDetector;
        private SpriteRenderer _spriteRenderer;
        private GameObject _target;

        public void Start()
        {
            _distanceCalculator = GetComponent<IDistanceCalculator>();
            _targetDetector = GetComponent<ITargetDetector>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // _weaponControler = GetComponent<WeaponControler>();
            Initialize(new IdleAttackState(this));
        }

        public void SelectRangeWeapon() {}
        public void SelectMeleeWeapon() {}
        public float CalculateDistanceToTarget()
        {
            _target = _targetDetector.DetectTarget();
            return _distanceCalculator.CalculateDistanceToTarget(_target);
        }

        public void SetSpriteColor(Color color) => _spriteRenderer.color = color;

        public void Attack()
        {
            // _weaponControler.MouseAim(_target.transform.position);
            // _weaponControler.PrimaryFireUp();
            // _weaponControler.PrimaryFireDown();
        }
    }
}
