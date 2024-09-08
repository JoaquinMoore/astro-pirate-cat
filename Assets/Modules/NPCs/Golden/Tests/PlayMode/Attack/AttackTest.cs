using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assets.Modules.NPCs.Golden.Attack;

namespace Assets.Modules.NPCs.Golden.Tests.PlayMode.Attack
{
    public class AttackTest
    {
        private AttackContext _attackContext;
        private MockDistanceCalculator _distanceCalculator;
        private MockTargetDetector _targetDetector;
        private MockWeaponsController _weaponsController;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            EditorSceneManager.LoadSceneAsyncInPlayMode(
                "Assets/Modules/NPCs/Golden/Tests/PlayMode/Rebuilds/AttackStates.unity",
                new LoadSceneParameters(LoadSceneMode.Single)
            );
            yield return new WaitForSeconds(1);
            _attackContext = Object.FindObjectOfType<AttackContext>();
            _distanceCalculator = _attackContext.GetComponent<MockDistanceCalculator>();
            _targetDetector = _attackContext.GetComponent<MockTargetDetector>();
            _weaponsController = _attackContext.GetComponent<MockWeaponsController>();
        }

        [UnityTest]
        public IEnumerator TestIdelAttackState()
        {
            _distanceCalculator.DistanceToReturn = _attackContext.RangeWeaponDistance + 1;
            _weaponsController.Reset();
            yield return null;
            Assert.AreEqual(0, _weaponsController.AttackMethodCalls);
        }

        [UnityTest]
        public IEnumerator TestMeleeAttackTest()
        {
            _distanceCalculator.DistanceToReturn = _attackContext.MeleeWeaponDistance - 1;
            _targetDetector.Target = new GameObject();
            _weaponsController.Reset();
            yield return null;
            Assert.AreEqual(_weaponsController.SelectMeleeWeaponMethodCalls, 1);
            Assert.AreEqual(_weaponsController.SetTargetMethodCalls.Count, 1);
            Assert.AreEqual(_targetDetector.Target, _weaponsController.SetTargetMethodCalls[0].Target);
            Assert.AreEqual(_weaponsController.AttackMethodCalls, 1);
        }

        [UnityTest]
        public IEnumerator TestRangeAttackTest()
        {
            _distanceCalculator.DistanceToReturn = _attackContext.MeleeWeaponDistance + 1;
            _targetDetector.Target = new GameObject();
            _weaponsController.Reset();
            yield return null;
            Assert.AreEqual(_weaponsController.SelectRangeWeaponMethodCalls, 1);
            Assert.AreEqual(_weaponsController.SetTargetMethodCalls.Count, 1);
            Assert.AreEqual(_targetDetector.Target, _weaponsController.SetTargetMethodCalls[0].Target);
            Assert.AreEqual(_weaponsController.AttackMethodCalls, 1);
        }
    }
}
