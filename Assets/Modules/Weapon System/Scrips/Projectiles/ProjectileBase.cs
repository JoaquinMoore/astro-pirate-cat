using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public enum CollisionBehaviour
    {
        None,
        Destroy,
        Deactivate,
        Stop
    }

    public CollisionBehaviour collisionBehaviour;
    public IProjectileCommand collisionCommand;
    public float movementSpeed;
    public float lifetime = 5f;

    private Vector2 _movementDirection;
    private Rigidbody2D _rigidBody;
    private Timer _lifeTimer;

    public void Shoot(Vector2 origin, Vector2 direction)
    {
        transform.position = origin;
        _movementDirection = direction;
        _lifeTimer.Restart();
        gameObject.SetActive(true);
    }

    public void Stop()
    {
        switch (collisionBehaviour)
        {
            case CollisionBehaviour.Destroy:
                Destroy(gameObject);
                break;
            case CollisionBehaviour.Deactivate:
                gameObject.SetActive(false);
                break;
            case CollisionBehaviour.Stop:
                _movementDirection = Vector2.zero;
                break;
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _lifeTimer = new Timer(lifetime);
    }

    private void Update()
    {
        if (_lifeTimer.Tick())
        {
            Stop();
        }
    }

    private void FixedUpdate()
    {
        var newPosition = _rigidBody.position + movementSpeed * Time.fixedDeltaTime * _movementDirection.normalized;
        _rigidBody.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCommand?.Execute(this, collision);
    }
}
