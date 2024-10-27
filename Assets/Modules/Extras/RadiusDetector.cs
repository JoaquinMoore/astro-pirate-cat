using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RadiusDetector
{
    public static IEnumerable<Transform> CircleDetector(Vector2 center, float radius, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(center, radius, layerMask).
            Where(c => c != null).
            Select(c => c.transform);
    }
}
