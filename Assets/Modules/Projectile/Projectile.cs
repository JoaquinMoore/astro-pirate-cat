using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private enum CollisionBehaviour
    {
        None,
        Destroy,
        Stop,
        Dissable
    }


    public UnityEvent<Collider2D, GameObject> OnCollide;

    [SerializeField]
    private Vector2 _currentVelocity;
    [SerializeField]
    private CollisionBehaviour _collisionBehaviour;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = _rigidBody.position + _currentVelocity * Time.fixedDeltaTime;
        _rigidBody.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollide.Invoke(collision, gameObject);

        switch (_collisionBehaviour)
        {
            case CollisionBehaviour.Destroy:
                Destroy(gameObject);
                break;
            case CollisionBehaviour.Stop:
                _currentVelocity = Vector2.zero;
                break;
            case CollisionBehaviour.Dissable:
                gameObject.SetActive(false);
                break;
        }
    }
}
