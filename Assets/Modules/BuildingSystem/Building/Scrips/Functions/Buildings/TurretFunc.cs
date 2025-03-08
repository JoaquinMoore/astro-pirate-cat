using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using WeaponSystem;

namespace BuildSystem
{
    public class TurretFunc
    {




        BuildingControler _model;

        LayerMask _enemyLayerMask;
        float _viewRange;
        float _lookingAngle;
        float _fieldOfView;
        bool _lockTurretRotation;


        WeaponControler _WeaponHandler;

        private IEnumerable<Transform> _EnemiesInSight => RadiusDetector.CircleDetector(
            _model._base.transform.position,
            _viewRange,
            _enemyLayerMask
        ).Select(c => c.transform);

        private Transform _NearEnemy
        {
            get
            {
                if (_EnemiesInSight.Any())
                    return _EnemiesInSight.OrderBy(e => Vector2.Distance(_model._base.transform.position, e.position)).First();
                else
                    return null;
            }
        }

        [System.Serializable]
        public class Settings
        {
            public WeaponControler _ref;
            public RangeWeaponData _wepData;
            public LayerMask EnemyLayerMask;
            public float ViewRange;
            [Range(0, 360)] public float LookingAngle;
            public float FieldOfView;
            public bool LockTurretRotation;
        }
        public TurretFunc(Settings settings, BuildingControler model)
        {
            _model = model;
            _WeaponHandler = settings._ref;
            _WeaponHandler.AddWeapons(settings._wepData);
            _WeaponHandler.EquipFirstWep();
            _enemyLayerMask = settings.EnemyLayerMask;
            _viewRange = settings.ViewRange;
            _lookingAngle = settings.LookingAngle;
            _fieldOfView = settings.FieldOfView;
            _lockTurretRotation = settings.LockTurretRotation;


        }
        public void Stop()
        {
            _WeaponHandler.PrimaryFireUp();
            _WeaponHandler.gameObject.SetActive(false);
        }
        public void Resume()
        {
            _WeaponHandler.gameObject.SetActive(true);
        }

        public void FindEnemies()
        {


            if (_model._base._Col.enabled && _NearEnemy != null)
                TargetEnemy();


            if (!_lockTurretRotation)
                return;
                //_WeaponHandler.MouseAim(new Vector2(-1f, -0.002f) + (Vector2)_WeaponHandler.transform.position);
            if (_lookingAngle == 270)
                _WeaponHandler.MouseAim(new Vector2(-1f, -0.002f) + (Vector2)_WeaponHandler.transform.position);
            else
                _WeaponHandler.MouseAim((GetVector2(_lookingAngle)) + (Vector2)_WeaponHandler.transform.position);
            //_WeaponHandler.MouseAim((GetVector2(_lookingAngle)) + (Vector2)_WeaponHandler.transform.position);
        }

        public Vector2 GetVector2(float angle)
        {
            Vector2 V2angle = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            return V2angle;
        }






        

        public void TargetEnemy()
        {
            if (!InFieldOfView(_NearEnemy.position) && _NearEnemy != null && _WeaponHandler != null)
            {
                _WeaponHandler.PrimaryFireUp();
                return;
            }
            
            
            _WeaponHandler.PrimaryFireDown();
            if (!_lockTurretRotation)
                _WeaponHandler.MouseAim(_NearEnemy.position);

        }



        public bool InFieldOfView(Vector3 targetPos)
        {
            Vector3 dir = targetPos - _model._base.transform.position;
            if (dir.magnitude > _viewRange) return false;
            return Vector3.Angle(GetVector2(_lookingAngle), dir) <= _fieldOfView / 2;
        }


        public bool InLineOfSight(Vector3 start, Vector3 end)
        {
            Vector3 dir = end - start;
            return !Physics.Raycast(start, dir, dir.magnitude, _enemyLayerMask);
        }
    }
}