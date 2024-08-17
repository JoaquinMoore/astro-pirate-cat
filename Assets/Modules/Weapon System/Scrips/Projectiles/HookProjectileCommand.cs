using Core.HookTool;
using UnityEngine;

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
            if (_tool.Grab(anchor, projectile.gameObject)) projectile.Stop();
        }
    }
}
