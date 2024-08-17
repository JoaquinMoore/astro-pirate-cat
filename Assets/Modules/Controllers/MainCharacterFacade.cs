using Core.HookTool;
using UnityEngine;

[RequireComponent(typeof(HookTool))]
public class MainCharacterFacade : MonoBehaviour
{
    private HookTool _hookTool;
    private int _anchorLayer;

    private void Awake()
    {
        _anchorLayer = LayerMask.GetMask("Anchor");
        _hookTool = GetComponent<HookTool>();
    }

    public void TryGrab(Vector2 origin, float raidus)
    {
        var collision = Physics2D.OverlapCircle(origin, raidus, _anchorLayer);
        if (!collision || !collision.TryGetComponent<HookAnchor>(out var anchor)) return;

        _hookTool.Grab(anchor);
    }
}

