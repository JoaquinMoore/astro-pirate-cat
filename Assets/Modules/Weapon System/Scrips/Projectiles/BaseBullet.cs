using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace WeaponSystem
{
    public class BaseBullet : BaseProjectile
    {
        protected IObjectPool<BaseBullet> _bulletsPool;
        public IObjectPool<BaseBullet> BulletsPool { set => _bulletsPool = value; }

        protected Rigidbody2D _rigidBody;
        protected CircleCollider2D _circleCollider;
        protected GameObject _particle;
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
        public virtual void ResetBullet() { _bulletsPool.Release(this); }
    }
}