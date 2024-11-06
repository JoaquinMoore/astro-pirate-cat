using HookToolSystem;
using System.Linq;
using UnityEngine;

public class MaincharacterController : MonoBehaviour, IMaincharacterController
{
    public void Impulse(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void Hook(Collider2D target, GameObject hookHead)
    {
        _hookTool.Grab(target, hookHead);
    }

    public void Shoot()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var targets = Physics2D.OverlapCircleAll(mousePos, 1f);

            if (targets.Any())
            {
                var target = targets.OrderBy(c => Vector2.Distance(c.transform.position, mousePos)).First();
                _hookTool.Grab(target, null);
            }
        }
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
    }

    private HookTool _hookTool;
    private Rigidbody2D _rigidBody;
}
