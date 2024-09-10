using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Attack.Interfaces
{
    public interface IDistanceCalculator
    {
        public float CalculateDistanceToTarget(GameObject obj2);
    }
}
