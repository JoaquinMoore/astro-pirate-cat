using System.Linq;
using UnityEngine;

public class MainCharacterPlayerBrain : MonoBehaviour
{
    public float radius;

    private void Awake()
    {
        _controller = GetComponent<MainCharacterController>();
    }

    private void Update()
    {
        // Detectar algo para enganchar.
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var collisions = Physics2D.OverlapCircleAll(_mousePosition, radius);
        if (collisions.Any())
        {
            var collision = collisions.OrderBy(x => Vector2.Distance(x.transform.position, _mousePosition)).First();
            _controller.Hook(collision, collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_mousePosition, radius);
    }

    private Vector2 _mousePosition;
    private IMainCharacterController _controller;
}
