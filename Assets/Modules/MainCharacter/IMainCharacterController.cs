using UnityEngine;

public interface IMainCharacterController
{
    void Jump();
    void Hook(Collider2D collider, GameObject hook);
}
