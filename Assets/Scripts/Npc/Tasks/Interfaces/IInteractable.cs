using UnityEngine;

namespace Npc.Tasks.Interfaces
{
    public interface IInteractable
    {
        float MinDistance { get; }
        Vector2 Position { get; }

        public Vector2 GetInteractionPosition(Vector2 interactorPosition) => Position + (interactorPosition - Position).normalized * MinDistance;

        void Interact(IInteractor visitor);
    }
}