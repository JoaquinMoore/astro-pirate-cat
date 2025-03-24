using UnityEngine;

public interface IMaincharacterController
{
    void Attack(bool value);
    void Hook(Vector3 target);
    void UnHook();
    void AimTo(Vector2 target);
    void SwapPrimWeapon();
    void ScrollWeapons(float rot);
    bool Hooking();


}
