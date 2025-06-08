using UnityEngine;

namespace _EXTENSIONS
{
    public static class Math
    {
        private const float EPSILON = 0.1f;

        public static bool AreApproximately(this Vector2 a, Vector2 b, float threshold = EPSILON)
        {
            return (a - b).magnitude <= threshold;
        }

        public static bool AreApproximately(this Vector3 a, Vector3 b, float threshold = EPSILON)
        {
            return (a - b).magnitude <= threshold;
        }
    }
}