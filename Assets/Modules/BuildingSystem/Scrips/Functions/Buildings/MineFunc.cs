using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

namespace BuildSystem
{
    public class MineFunc
    {
        BuildingControler _controler;


        float _explosionRaduis;
        float _detectionRaduis;
        int _explosionDamage;
        float _explosionTimer;
        int _layerID;
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
            public int LayerID;
        }

        public MineFunc(Settings settings, BuildingControler model)
        {
            _controler = model;

            _explosionRaduis = settings.ExplosionRaduis;
            _detectionRaduis = settings.DetectionRaduis;
            _explosionDamage = settings.ExplosionDamage;
            _explosionTimer = settings.ExplosionTimer;
            _layerID = settings.LayerID;
        }

        public void CheckNearby()
        {
            var holder = Physics2D.CircleCastAll(_controler._base.transform.position, _detectionRaduis, Vector2.zero);
            foreach (var item in holder)
            {

                if (_AllEnemys.Contains(item.collider.gameObject))
                    continue;

                if (item.collider.gameObject.layer == _layerID && item.collider.gameObject.GetComponent<Collider2D>().enabled)
                {
                    _AllEnemys.Add(item.collider.gameObject);
                }
            }

            foreach (var item in _AllEnemys)
            {
                if (Vector2.Distance(item.transform.position, _controler._base.transform.position) <= _explosionRaduis)
                    _Triggered = true;


            }
            Debug.Log(_AllEnemys.Count);
            if (!_Triggered)
                return;

            DamageAll();
        }

        public void DamageAll()
        {
            _Timer += Time.deltaTime;

            if (_Timer >= _explosionTimer)
            {
                _controler._visual.Destroy(true);
                _Triggered = false;
                foreach (var item in _AllEnemys)
                {
                    if (item == null)
                        continue;
                    var hold = item.GetComponent<Health>();
                    if (hold == null)
                        continue;

                    hold.Hurt(_explosionDamage);

                    //item.SetActive(false);
                }
                _AllEnemys.Clear();
                _Timer = 0;
            }
        }


    }
}