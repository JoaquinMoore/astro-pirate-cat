using UnityEngine;

public class Barco : MonoBehaviour, IInteractable
{
    public float MinDistance => 5;
    public Vector2 Position => transform.position;

    public void Interact(IInteractor visitor)
    {
        visitor.Interact(this);
    }

    public void Hurt(int damage)
    {
        Debug.Log(damage);
    }
}