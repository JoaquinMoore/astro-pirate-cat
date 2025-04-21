using UnityEngine;

public class Boid : MonoBehaviour, IBoid
{
    public Vector2 Position => transform.position;
}
