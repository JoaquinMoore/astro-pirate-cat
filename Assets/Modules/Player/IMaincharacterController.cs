using UnityEngine;

public interface IMaincharacterController
{
    void Shoot();
    void Hook(Collider2D target, GameObject hookHead);
}
