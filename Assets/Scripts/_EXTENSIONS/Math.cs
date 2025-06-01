using UnityEngine;

namespace _EXTENSIONS
{
    public static class Math
    {
        public static bool AreApproximately(this Vector2 a, Vector2 b, float threshold)
        {
            return (a - b).magnitude <= threshold;
        }

        public static bool AreApproximately(this Vector3 a, Vector3 b, float threshold)
        {
            return (a - b).magnitude <= threshold;
        }
    }
}