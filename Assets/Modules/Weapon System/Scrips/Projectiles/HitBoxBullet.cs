using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class HitBoxBullet : BaseBullet
    {
        protected List<IHurtable> _targets = new();




        private bool visual;
        private int _damage;
        private int _maxTargets;

        protected List<string> _targetsTag;
        public override void SetUp(BaseBulletData data)
        {
            visual = false;
            HitBoxBulletData _data = data as HitBoxBulletData;

            _particle = _data.ParticlePref;
            _maxTargets = _data.MaxTargets;
            _damage = _data.Damage;
            gameObject.SetActive(false);
            VisualSetUp(data);
        }

        public virtual void VisualSetUp(BaseBulletData data)
        {

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
            Debug.Log(_targets.Count);

            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == null)
                {
                    _targets.Remove(_targets[i]);
                    continue;
                }

                _targets[i].Hurt(_damage);
            }

            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == null)
                {
                    _targets.Remove(_targets[i]);
                    continue;
                }
            }

            ExternalInputVisual();
        }
        public virtual void ExternalInputVisual()
        {

        }


        public void Update()
        {
            if (visual)
                Visual();
        }

        public virtual void Visual()
        {


        }

<<<<<<< HEAD
        public virtual void UpdateTartetListVisual()
        {

        }

=======
>>>>>>> main

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hold = collision.GetComponent<IHurtable>();
            if (_maxTargets <= _targets.Count)
                return;

            if (hold != null && !_targets.Contains(hold))
            {
                _targets.Add(hold);
            }
<<<<<<< HEAD
            UpdateTartetListVisual();
=======
>>>>>>> main
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var hold = collision.GetComponent<IHurtable>();
            if (_maxTargets <= _targets.Count)
                return;

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
<<<<<<< HEAD
            UpdateTartetListVisual();
=======
>>>>>>> main
        }


    }
}