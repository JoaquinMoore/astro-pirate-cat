using UnityEngine;

public interface IInteractable
{
    float MinDistance { get; }
    Vector2 Position { get; }
    void Interact(IInteractor visitor);

    Vector2 GetInteractionPosition(Vector2 interactorPosition) => Position + (interactorPosition - Position).normalized * MinDistance;
}