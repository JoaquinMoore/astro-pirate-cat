using Core.HookTool;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public IProjectileCommand collisionCommand;
    private Rigidbody2D _rigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCommand?.Execute(this, collision);
    }
}

public interface IProjectileCommand
{
    void Execute(ProjectileBase projectile, Collider2D collision);
}

public class HookProjectileCommand : IProjectileCommand
{
    private HookTool _tool;

    public HookProjectileCommand(HookTool tool)
    {
        _tool = tool;
    }

    public void Execute(ProjectileBase projectile, Collider2D collision)
    {
        if (collision.TryGetComponent<HookAnchor>(out var anchor))
        {
            _tool.Grab(anchor, projectile.gameObject);
        }
    }
}
