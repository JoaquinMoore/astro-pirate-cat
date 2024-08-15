using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class LaserBullet : BaseBullet
    {
        protected List<IHurtable> _targets = new();
        [SerializeField] private LineRenderer _lr;
        [SerializeField] private float VisualLenght;
        private bool visual;
        private int _damage;
        private int _maxTargets;

        protected List<string> _targetsTag;
        public override void SetUp(BaseBulletData data)
        {
            visual = false;
            LaserBulletData _data = data as LaserBulletData;

            _maxTargets = _data.MaxTargets;
            _damage = _data.Damage;
            Debug.Log(_data.Damage);
            //_lr.GetComponentInChildren<LineRenderer>();
            gameObject.SetActive(false);

        }
        private void OnDisable()
        {
            _targets.Clear();
            visual = false;
        }
        private void OnEnable()
        {
            visual = true;
        }
        public override void ExternalInput()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == null)
                    continue;
                _targets[i].Hurt(_damage);
            }

        }

        public void Update()
        {
            if (visual)
                Visual();
        }

        public void Visual()
        {
            if (_lr == null)
                return;
            _lr.SetPosition(0, transform.position);
            _lr.SetPosition(1, transform.position + (transform.right * VisualLenght));
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hold = collision.GetComponent<IHurtable>();
            if (hold != null && !_targets.Contains(hold))
            {
                _targets.Add(hold);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var hold = collision.GetComponent<IHurtable>();

            if (hold != null && _targets.Contains(hold))
            {
                _targets.Remove(hold);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position+(transform.right * VisualLenght));
        }
    }
}