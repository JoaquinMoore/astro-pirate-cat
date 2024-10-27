using UnityEngine;
using Assets.Modules.NPCs.Golden.Attack.Interfaces;

namespace Assets.Modules.NPCs.Golden.Tests.PlayMode.Attack
{
    public class MockDistanceCalculator : MonoBehaviour, IDistanceCalculator
    {
        public static MockDistanceCalculator Configure(float distanceToReturn) => new MockDistanceCalculator(distanceToReturn);

        public float DistanceToReturn { get; set; }

        public MockDistanceCalculator(float distanceToReturn) => DistanceToReturn = distanceToReturn;

        public float CalculateDistanceToTarget(GameObject target) => DistanceToReturn;
    }
}
