using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class MineFunc
    {
        BuildingControler _controler;


        float _explosionRaduis;
        float _detectionRaduis;
        int _explosionDamage;
        float _explosionTimer;
        string _tag;
        private List<GameObject> _AllEnemys = new();
        //private List<Joaquin.Enemies.Enemy> _AllEnemys = new ();
        private float _Timer;
        private bool _Triggered;
        [System.Serializable]
        public class Settings
        {
            public float ExplosionRaduis;
            public float DetectionRaduis;
            public int ExplosionDamage;
            public float ExplosionTimer;
            public string Tag;
        }

        public MineFunc(Settings settings, BuildingControler model)
        {
            _controler = model;

            _explosionRaduis = settings.ExplosionRaduis;
            _detectionRaduis = settings.DetectionRaduis;
            _explosionDamage = settings.ExplosionDamage;
            _explosionTimer = settings.ExplosionTimer;
            _tag = settings.Tag;
        }

        public void CheckNearby()
        {
            var holder = Physics2D.CircleCastAll(_controler._base.transform.position, _detectionRaduis, Vector2.zero);
            foreach (var item in holder)
            {

                if (_AllEnemys.Contains(item.collider.gameObject))
                    continue;

                if (item.collider.tag == _tag && item.collider.enabled)
                {
                    _AllEnemys.Add(item.collider.gameObject);
                }
            }

            foreach (var item in _AllEnemys)
            {
                if (Vector2.Distance(item.transform.position, _controler._base.transform.position) <= _explosionRaduis)
                    _Triggered = true;
            }

            if (!_Triggered)
                return;

            DamageAll();
        }

        public void DamageAll()
        {
            _Timer += Time.deltaTime;

            if (_Timer >= _explosionTimer)
            {
                _Triggered = false;
                foreach (var item in _AllEnemys)
                {
                    //if (item.death == true)
                    //{
                    //    _AllEnemys.Clear();
                    //    return;
                    //}
                    //else
                    //    item.Damage(_explosionDamage);
                    item.GetComponent<BoxCollider2D>().enabled = false;
                    item.SetActive(false);
                }
                _controler._model.Death();
                _AllEnemys.Clear();
                _Timer = 0;
            }
        }


    }
}