using UnityEngine;
using UnityEngine.InputSystem;

namespace _EXTENSIONS
{
    public static class MouseExtensions
    {
        private static Camera s_mainCamera;

        private static Camera MainCamera => s_mainCamera ??= Camera.main;

        public static Vector2 WorldPosition(this Mouse mouse)
        {
            return MainCamera.ScreenToWorldPoint(mouse.position.value);
        }
    }
}