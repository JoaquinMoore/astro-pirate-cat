using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class StandardBullet : BaseBullet
    {
        [Header("Visual")]
        [SerializeField]private SpriteRenderer _mat;
        public override IEnumerator OnInpact() { yield return null; Destroy(gameObject); StopAllCoroutines(); }

        protected int _damage;
        protected float _speed;
        protected float _timer;

        protected List<string> _targetsTag;

        private void OnEnable()
        {
            if (_timer != 0)
                StartCoroutine(DespawnTimer());
        }

        public override void SetUp(BaseBulletData data, BaseMagazine action)
        {
            StandardBulletData _data = data as StandardBulletData;

            _damage = _data._damage;
            _speed = _data._speed;
            _timer = _data._timer;
            if (_mat == null)
                GetComponent<SpriteRenderer>().material = _data.AlainceMat;
            else
               _mat.material = _data.AlainceMat;
            StartCoroutine(DespawnTimer());
            base.SetUp(data, action);
        }
        public override void Move()
        {
            _rigidBody.position += (Vector2)_rigidBody.transform.right * Time.fixedDeltaTime * _speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hold = collision.GetComponent<IHurtable>();

            if (hold != null)
            {
                hold.Hurt(_damage);
                OnImpact();
                ResetBullet();
            }
        }

        public override void ResetBullet()
        {

            base.ResetBullet();
        }

        public virtual void OnImpact() { }


        public override IEnumerator DespawnTimer() { yield return new WaitForSeconds(_timer); ResetBullet(); }

    }
}