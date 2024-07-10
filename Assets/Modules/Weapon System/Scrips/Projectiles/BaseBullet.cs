using System.Collections;
using UnityEngine;

public class BaseBullet : BaseProjectile
{
    protected Rigidbody2D _rigidBody;
    protected CircleCollider2D _circleCollider;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public virtual void SetUp(BaseBulletData data) { }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void ExternalInput() { }
    public virtual IEnumerator DespawnTimer() { yield return null; }
}
