using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    protected Rigidbody2D _rigidBody;
    protected CircleCollider2D _circleCollider;

    protected int _damage;
    protected float _speed;
    protected float _timer;
    protected List<string> _targetsTag;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public virtual void SetUp(int damage, float speed, float timer)
    {
        _damage = damage;
        _speed = speed;
        _timer = timer;
        StartCoroutine(DespawnTimer());
    }

    private void FixedUpdate()
    {
        Move();
    }


    public virtual void OnImpact() { Destroy(gameObject); StopAllCoroutines(); }
    public virtual void Move() { }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!_targetsTag.Contains(collision.tag))
        //    return;
        //
        ////if (collision.TryGetComponent<IDamageable>(out var component))
        ////    component.Damage(_damage);
        //
        //OnImpact();
    }
    public IEnumerator DespawnTimer() { yield return new WaitForSeconds(_timer); Destroy(gameObject); }

}
