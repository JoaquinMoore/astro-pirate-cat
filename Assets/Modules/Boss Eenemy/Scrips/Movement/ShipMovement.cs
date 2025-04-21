using HealthSystem;
using System.Collections;
using UnityEngine;


namespace NPC.Boss.Ship
{
    public class ShipMovement
    {
        MoveSettings _settings;
        Vector3 _startingPos;
        Vector3 _MovePos;
        ShipEnemy _ship;

        float _sinTime;

        float _agroDistance;


        float _curspeed;
        bool _stop;

        public ShipMovement(MoveSettings settings, ShipEnemy ship, float agroDistance)
        {
            _settings = settings;
            _ship = ship;

            if (_settings.Target != null)
            {
                _startingPos = _settings.Target.position;
                _MovePos = _settings.Target.position;
            }
            else
                _startingPos = _ship.transform.position;

            _curspeed = settings.Speed;
            _agroDistance = agroDistance;
        }


        public void Movement()
        {

            if (_stop == true)
                return;

            if (_MovePos == Vector3.zero && !GameManager.Instance.CheckPlayerDistance(_startingPos, _agroDistance))
                return;


            if (_MovePos == Vector3.zero)
            {
                SetPos();
                return;
            }

            if (Vector3.Distance(_ship.transform.position, _MovePos) <= _settings.MinDistanceToTarget)
            {
                _MovePos = Vector3.zero;
                _sinTime = 0;
                return;
            }
            float hpcurrent = _curspeed * _settings.HpCurve.Evaluate((float)_settings.Hp.CurrentHealth / _settings.Hp.PublicMaxHealth);

            _ship.Anim.SetFloat("Blend", (float)_settings.Hp.CurrentHealth / _settings.Hp.PublicMaxHealth);



            if ((float)_settings.Hp.CurrentHealth / _settings.Hp.PublicMaxHealth <= 0.5)
            {
                _ship._stop = true;
                _ship.Anim.SetBool("IsMove", false);
                _ship.Anim.SetBool("StopMove", true);
            }


            if (hpcurrent <= 0)
            {
                _ship._stop = true;
                _ship.Anim.SetBool("IsMove", false);
            }


            Vector2 pos = (Vector2)_MovePos - (Vector2)_ship.transform.position;

            _settings.RotRef.transform.right = Vector2.Lerp(_settings.RotRef.transform.right, pos, Time.deltaTime * _settings.RotSpeed);

            _ship.transform.position += _settings.RotRef.transform.right * hpcurrent * Time.deltaTime;



            //test3
            //Debug.Log(hpcurrent);
            ////Debug.Log((float)_settings.Hp.PublicCurrentHealth / _settings.Hp.PublicMaxHealth);
            //_ship.transform.position = Vector3.MoveTowards(_ship.transform.position, Vector3.Lerp(_ship.transform.position, _MovePos, hpcurrent * Time.deltaTime), hpcurrent * Time.deltaTime);


            //test2
            //_sinTime += Time.deltaTime * _settings.Speed;
            //_sinTime = Mathf.Clamp(_sinTime, 0, Mathf.PI);
            //float t = 0.5f * Mathf.Sin(_sinTime - Mathf.PI / 2f) + 0.5f;
            //_ship.transform.position = Vector3.Lerp(_ship.transform.position, _MovePos, t);

            //test1
            //_ship.transform.position = Vector3.MoveTowards(_ship.transform.position, _MovePos, _settings.Speed * Time.deltaTime);
        }

        public void StopMov()
        {
            _ship.StartCoroutine(DeSpeedMod());
        }

        public void ResumeMov()
        {
            _ship.StartCoroutine(IncSpeedMod());

        }

        public bool Stoped()
        {
            return _curspeed == 0;
        }


        IEnumerator IncSpeedMod()
        {
            _stop = false;
            while (_curspeed < _settings.Speed)
            {
                Debug.Log(_curspeed);
                yield return new WaitForSeconds(0.05f);
                _curspeed += _settings.Speed_ModPerTic;
            }
            _curspeed = _settings.Speed;

        }


        IEnumerator DeSpeedMod()
        {
            _ship.Anim.SetBool("IsMove", false);
            while (_curspeed > 0)
            {
                Debug.Log(_curspeed);
                yield return new WaitForSeconds(0.05f);
                _curspeed -= _settings.Speed_ModPerTic;
            }
            _curspeed = 0;

            _stop = true;
        }



        public void SetPos()
        {
            Vector3 pos = (Vector3)(Random.insideUnitCircle * _settings.MaxTargetDistane);
            if (Vector3.Distance(_startingPos, _startingPos + pos) <= _settings.MinTargetDistane)
            {
                SetPos();
                return;
            }
            if (Vector3.Distance(_ship.transform.position, _startingPos + pos) <= _settings.MinDistanceToTarget)
            {
                SetPos();
                return;
            }


            _MovePos = _startingPos + pos;

        }


        public void SetTarget(Transform target)
        {
            _startingPos = target.position;
            _MovePos = target.position;
        }



        [System.Serializable]
        public class MoveSettings
        {
            [Header("Hp Settings")]
            public AnimationCurve HpCurve;
            public Health Hp;

            [Header("Movement Settings")]
            [HideInInspector] public Transform Target;
            public Transform RotRef;
            public float MinTargetDistane;
            public float MaxTargetDistane;
            public float MinDistanceFromShip;
            public float Speed;
            public float Speed_ModPerTic;
            public float RotSpeed;
            [Header("Movement Visual Settings")]
            public float MinDistanceToTarget;
        }
    }
}