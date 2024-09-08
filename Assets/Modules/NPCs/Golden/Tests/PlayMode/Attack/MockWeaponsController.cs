using UnityEngine;
using Assets.Modules.NPCs.Golden.Attack.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Assets.Modules.NPCs.Golden.Tests.PlayMode.Attack
{
    public class MockWeaponsController : MonoBehaviour, IWeaponsController
    {
        public struct TargetMethodCall
        {
            public GameObject Target;
        }

        private List<TargetMethodCall> _targetMethodCalls = new();

        public int AttackMethodCalls { get; private set; } = 0;
        public int SelectMeleeWeaponMethodCalls { get; private set; } = 0;
        public int SelectRangeWeaponMethodCalls { get; private set; } = 0;
        public ReadOnlyCollection<TargetMethodCall> SetTargetMethodCalls => _targetMethodCalls.AsReadOnly();

        public void Reset()
        {
            AttackMethodCalls = 0;
            SelectMeleeWeaponMethodCalls = 0;
            SelectRangeWeaponMethodCalls = 0;
            _targetMethodCalls.Clear();
        }

        public void Attack() => AttackMethodCalls++;
        public void SelectMeleeWeapon() => SelectMeleeWeaponMethodCalls++;
        public void SelectRangeWeapon() => SelectRangeWeaponMethodCalls++;
        public void SetTarget(GameObject target) => _targetMethodCalls.Add(new TargetMethodCall { Target = target });
    }
}
