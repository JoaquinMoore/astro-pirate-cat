using UnityEngine;
using UnityEngine.InputSystem;

namespace Extensions
{
    public static class MouseExtensions
    {
        public static Vector2 WorldPosition(this Mouse mouse)
        {
            return Camera.main.ScreenToWorldPoint(mouse.position.value);
        }
    }
}
