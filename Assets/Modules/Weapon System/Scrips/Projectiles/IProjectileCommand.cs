using UnityEngine;

public interface IProjectileCommand
{
    void Execute(ProjectileBase projectile, Collider2D collision);
}
