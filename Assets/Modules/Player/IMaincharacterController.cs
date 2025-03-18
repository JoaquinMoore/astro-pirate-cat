using UnityEngine;

public interface IMaincharacterController
{
    void Attack(bool value);
    void Hook(Collider2D target, GameObject hookHead = null);
    void AimTo(Vector2 target);
}
