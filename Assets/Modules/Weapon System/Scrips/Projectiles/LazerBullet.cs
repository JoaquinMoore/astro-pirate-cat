using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class LazerBullet : HitBoxBullet
    {
        [Header("Visual")]
        [SerializeField] private LineRenderer _lr;
        [SerializeField] private float VisualLenght;


        [SerializeField] private GameObject _EndVisual;

        [SerializeField] private BoxCollider2D _colider;

        private IHurtable _fardestTarget;
        [SerializeField] private float _fardestTargetDist;


        private void OnDisable()
        {
            _fardestTargetDist = 0;
        }

        public override void VisualSetUp(BaseBulletData data)
        {
            LazerBulletData _data = data as LazerBulletData;

            _colider = GetComponent<BoxCollider2D>();

            _colider.offset = new Vector2(VisualLenght /2,0);
            _colider.size = new Vector2(VisualLenght, _colider.size.y);
        }
        public override void Visual()
        {
            if (_lr == null)
                return;
            VisualLenghtCalc();

            float curdist = 0;

            if (_targets.Count > 0)
                curdist = _fardestTargetDist;
            else
                curdist = VisualLenght;


            _lr.SetPosition(0, transform.position);
            _lr.SetPosition(1, transform.position + (transform.right * curdist));

            if (_EndVisual != null)
                _EndVisual.transform.position = _lr.GetPosition(1);



        }


        public override void UpdateTartetListVisual()
        {
            float newdist = 0;
            foreach (var item in _targets)
            {
                float dist = Vector3.Distance(transform.position, item.getGameObject().transform.position);

                if (newdist < dist && _fardestTarget == null)
                {
                    newdist = dist;
                }


            }
            _fardestTargetDist = newdist;
        }

        public void VisualLenghtCalc()
        {
            if (_fardestTarget == null)
            {
                _fardestTargetDist = 0;
            }


            foreach (var item in _targets)
            {
                float dist = Vector3.Distance(transform.position, item.getGameObject().transform.position);

                if (_fardestTargetDist < dist)
                {
                    _fardestTargetDist = dist;
                    _fardestTarget = item;
                }


            }
        }



        public override void ExternalInputVisual()
        {
            foreach (var item in _targets)
            {
                Particle par = _baseMagRef.SpawnParticle(transform.transform);
                float distance = Vector3.Distance(transform.position, item.getGameObject().transform.position);

                par.transform.localPosition = transform.right + new Vector3(distance, 0,0);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + (transform.right * VisualLenght));
        }

    }
}