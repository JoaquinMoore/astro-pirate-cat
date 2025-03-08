using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RadiusDetector
{
    public static IEnumerable<Collider2D> CircleDetector(Vector2 center, float radius, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(center, radius, layerMask).Where(c => c != null);
    }

    public static IEnumerable<Collider2D> CircleDetector(Vector2 center, float radius)
    {
        return CircleDetector(center, radius, LayerMask.GetMask("Default"));
    }
}
