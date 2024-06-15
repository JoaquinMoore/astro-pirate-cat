using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMagazine
{
    Transform _ShootPivot;
    bool _CanFire = true;
    BaseBullet _projectile;
    RangeWeapon _wp;

    public virtual void AddData(Transform shootpivot, RangeWeapon wp) { _ShootPivot = shootpivot; _wp = wp; }
    public void TemporalAddBullet(BaseBullet bullet) { _projectile = bullet; }
    public virtual void Fire() { }
    public virtual void SpawnProjectile(Settings settings) 
    {
        BaseBullet hold = Object.Instantiate(_projectile);
        hold.transform.position = _ShootPivot.position;
        hold.transform.right  = _ShootPivot.right;
        hold.SetUp(settings.Damage, settings.Speed, settings._DespawnTime);
    }
    public virtual bool CanFire() { return _CanFire; }

    [System.Serializable]
    public class Settings
    {
        public GameObject Projectile;
        public int Damage;
        public float Speed = 10;
        public float _DespawnTime = 5;
    }
}

public class StandardMagazine : BaseMagazine
{
    StandarSettings settings = new();


    [System.Serializable]
    public class StandarSettings: Settings
    {

    }

    public override void AddData(Transform shootpivot,RangeWeapon wp)
    {




        base.AddData(shootpivot, wp);
    }

    public override void Fire()
    {
        Debug.Log("pew");
        SpawnProjectile(settings);
    }

}
