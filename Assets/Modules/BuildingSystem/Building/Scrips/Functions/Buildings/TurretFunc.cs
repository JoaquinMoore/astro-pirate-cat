using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        bool _coneView;

        //WeaponHandler _WeaponHandler;

        //private IEnumerable<Transform> _EnemiesInSight => RadiusDetector.CircleDetector(
        //    _model._base.transform.position,
        //    _viewRange,
        //    _enemyLayerMask
        //);

        //private Transform _NearEnemy
        //{
        //    //get
        //    //{
        //    //    //if (_EnemiesInSight.Any())
        //    //    //    return _EnemiesInSight.OrderBy(e => Vector2.Distance(_model._base.transform.position, e.position)).First();
        //    //    //else
        //    //    //    return null;
        //    //}
        //}


        public class Settings
        {
            //public WeaponHandler.Settings WeaponData;
            public LayerMask EnemyLayerMask;
            public float ViewRange;
            [Range(0, 360)] public float LookingAngle;
            public float FieldOfView;
            public bool LockTurretRotation;
            public bool ConeView;
        }
        public TurretFunc(Settings settings, BuildingControler model)
        {
            _model = model;
            //_WeaponHandler = settings.WeaponData;

            _enemyLayerMask = settings.EnemyLayerMask;
            _viewRange = settings.ViewRange;
            _lookingAngle = settings.LookingAngle;
            _fieldOfView = settings.FieldOfView;
            _lockTurretRotation = settings.LockTurretRotation;
            _coneView = settings.ConeView;

            //_WeaponHandler.AimWeaponTowards(GetVector2(LookingAngle) + (Vector2)transform.position);


        }

        public void FindEnemies()
        {
            //if (_model._base._Col.enabled && _NearEnemy != null)
            //    TargetEnemy();
        }





        public Vector2 GetVector2(float angle)
        {
            Vector2 V2angle = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            return V2angle;
        }

        public void TargetEnemy()
        {
            //if (!InFieldOfView(_NearEnemy.position))
            //    return;

            //_WeaponHandler.Trigger();
            //if (_lockTurretRotation)
            //_WeaponHandler.AimWeaponTowards(_NearEnemy.position);

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