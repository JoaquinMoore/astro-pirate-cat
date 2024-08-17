using UnityEngine;
using UnityEngine.InputSystem;

public static class MyExtensions
{
    public static Vector2 WorldPosition(this Mouse mouse)
    {
        return Camera.main.ScreenToWorldPoint(mouse.position.value);
    }
}
