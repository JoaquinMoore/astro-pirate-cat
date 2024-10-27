using Assets.Modules.NPCs.Golden.Attack.Interfaces;
using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Tests.PlayMode.Attack
{
    public class MockTargetDetector : MonoBehaviour, ITargetDetector
    {
        public GameObject Target { get; set; }

        public MockTargetDetector(GameObject target) => Target = target;

        public GameObject DetectTarget() => Target;
    }
}
