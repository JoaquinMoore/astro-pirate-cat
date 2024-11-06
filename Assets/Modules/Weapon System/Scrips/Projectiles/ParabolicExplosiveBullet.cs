using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{

    public class ParabolicExplosiveBullet : StandardBullet
    {
        [SerializeField] private GameObject _effect;
        [SerializeField] private float _distance;

        [SerializeField] private List<Vector3> _points;
        [SerializeField] private Vector3 _point;
        [SerializeField] private int _id;
        void Start()
        {

        }



        void Update()
        {

        }
        public void ReciveList(List<Vector3> points)
        {
            _points = points;
            _point = _points[1];
            Vector2 pos = _point - transform.position;
            transform.right = pos;
            transform.position = _points[0];
        }


        public override void Move()
        {
            if (CheckDistance(_point) && _id != _points.Count)
            {
                _point = _points[_id];
                _id++;
            }

            Vector2 pos = _point - transform.position;



            transform.right = Vector3.Lerp(transform.right, pos, Time.deltaTime * 10);
            _rigidBody.position += (Vector2)_rigidBody.transform.right * Time.fixedDeltaTime * _speed;

            if (_id == _points.Count)
            {
                OnImpact();
                ResetBullet();
                _id = 0;
            }
        }

        public bool CheckDistance(Vector3 target)
        {
            return Vector3.Distance(target,transform.position) < _distance;
        }



        public override void OnImpact()
        {
            Instantiate(_particle, transform.position, Quaternion.identity);
            Instantiate(_effect, transform.position, Quaternion.identity);

            base.OnImpact();
        }
    }
}