using Assets.Modules.NPCs.Golden.Movements.Interfaces;
using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Tests.PlayMode.Movements
{
    public class MockInput : MonoBehaviour, IInput
    {
        [field: SerializeField] public float HDirection { get; set; } = 0;
        [field: SerializeField] public float VDirection { get; set; } = 0;

        public float HorizontalDirection() => HDirection;
        public float VerticalDirection() => VDirection;
    }
}
