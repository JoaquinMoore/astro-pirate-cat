using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseMagazine
{
    public string name;
    protected Transform _ShootPivot;
    bool _CanFire = true;
    BaseBulletData _projectile;
    protected RangeWeapon _wp;
    protected float _recoil;
    float _spreadSpeedMod;
    float _maxSpread;
    float _currentSpread;

    public virtual void AddData(Transform shootpivot, RangeWeapon wp, Settings settings ) 
    { 
        _ShootPivot = shootpivot;
        _wp = wp; _projectile = settings.BulletData;
        _maxSpread = settings.MaxSpread;
        _spreadSpeedMod = settings.SpreadSpeedMod;
        _recoil = settings.Recoil;
    }
    public virtual void Fire() { _currentSpread = Mathf.Lerp(_currentSpread, _maxSpread, Time.deltaTime * _spreadSpeedMod); }
    public virtual void FireIsUp() { _currentSpread = 0; }
    public virtual void SpawnProjectile(Settings settings) 
    {
        BaseBullet hold = Object.Instantiate(_projectile.BulletPrefab);
        hold.transform.position = _ShootPivot.position;
        hold.transform.right  = _ShootPivot.right;
        hold.SetUp(_projectile);
    }
    public virtual bool CanFire() { return _CanFire; }
    public virtual float SpreadMaker() {return Random.Range(-_currentSpread, _currentSpread); }
    public virtual float RecoilReturn() { return _recoil; }
    public virtual void Reselect() { }
    public virtual void Reset() { }
    public virtual void ReEquip() { }


    public virtual BaseMagazine Clone()
    {
        return null;
    }
    public virtual Settings GiveSettings()
    {
        return null;
    }

    [System.Serializable]
    public class Settings
    {
        [Header("Base Settings")]
        public float SpreadSpeedMod = 1;
        public float MaxSpread;
        public float Recoil;
        public BaseBulletData BulletData;
    }
}
[System.Serializable]
public class StandardMagazine : BaseMagazine
{
    public StandarSettings settings = new();


    [System.Serializable]
    public class StandarSettings: Settings
    {

    }

    public override void AddData(Transform shootpivot,RangeWeapon wp, Settings settings)
    {
        base.AddData(shootpivot, wp, settings);
    }

    public override void Fire()
    {
        SpawnProjectile(settings);
        base.Fire();
    }


    public override BaseMagazine Clone()
    {
        return new StandardMagazine();
    }
    public override Settings GiveSettings()
    {
        return settings;
    }
}

[System.Serializable]
public class LaserHeatMagazine : BaseMagazine
{
    [SerializeField] private LaserHeatSettings _settings;
    BaseBullet _bulletHolder;


    [Header("Debug")]
    [SerializeField] private float _currentHeat;
    [SerializeField] float _currentTime;
    [SerializeField] float _currentRecoil;
    [SerializeField] bool _overHeat;

    [System.Serializable]
    public class LaserHeatSettings : Settings
    {
        [Header("Recoil")]
        public float RecoilSpeedGainMod = 1;

        [Header("Heat")]
        public float MaxHeat;
        public float HeatGainTime = 1;
        public AnimationCurve HeatGainCurve;

        [Header("Heat loose")]
        public float CoolDownRate = 1;
        public AnimationCurve HeatLoseCurve;
        public float CoolDownRatePerTic = 1;
        public float CoolDownStartDelay = 1;
        public float CoolDownOverheatExtraStartDelay = 1;
    }



    public override void AddData(Transform shootpivot, RangeWeapon wp, Settings settings)
    {
        _settings = settings as LaserHeatSettings;
        if (_bulletHolder == null)
        {
            _bulletHolder = Object.Instantiate(settings.BulletData.BulletPrefab, shootpivot);
            _bulletHolder.transform.position = shootpivot.position;
        }
        _bulletHolder.SetUp(settings.BulletData);




        base.AddData(shootpivot, wp, settings);
    }

    public override void Fire()
    {
        _wp.StopAllCoroutines();
        if (!_bulletHolder.gameObject.activeSelf)
        {
            _bulletHolder.gameObject.SetActive(true);
        }
        _currentTime += Time.fixedDeltaTime;
        float Pertage = _settings.HeatGainCurve.Evaluate(_currentTime / _settings.HeatGainTime);
        _currentHeat = Mathf.Lerp(0, _settings.MaxHeat, Pertage);

        _currentRecoil = Mathf.Lerp(_currentRecoil, _recoil, Time.deltaTime * _settings.RecoilSpeedGainMod);
        _bulletHolder.ExternalInput();
        base.Fire();
    }

    public override void FireIsUp()
    {
        if (_overHeat && _currentHeat != _settings.MaxHeat)
            return;

        _wp.StopAllCoroutines();
        _bulletHolder.gameObject.SetActive(false);
        _wp.StartCoroutine(CoolDown());
        _currentRecoil = 0;
        base.FireIsUp();
    }

    IEnumerator CoolDown()
    {
        float cooldown = 0;
        cooldown = _overHeat == false ? _settings.CoolDownStartDelay : _overHeat == true ? _settings.CoolDownOverheatExtraStartDelay : _settings.CoolDownStartDelay;
        yield return new WaitForSeconds(cooldown);

        if (_currentHeat == _settings.MaxHeat)
            _currentTime = _settings.HeatGainTime;
        float amount = _currentHeat;
        while (_currentHeat > 0)
        {
            yield return new WaitForSeconds(_settings.CoolDownRate);
            _currentTime -= Time.fixedDeltaTime;
            float Pertage = _settings.HeatGainCurve.Evaluate(_currentTime / _settings.HeatGainTime);
            _currentHeat = Mathf.Lerp(0, amount, Pertage);
            
        }
        _currentHeat = 0;
        _currentTime = 0;
        _overHeat = false;
    }

    public override bool CanFire()
    {
        if (_currentHeat >= _settings.MaxHeat)
            _overHeat = true;


        return !_overHeat;
    }

    public override void Reset()
    {
        _bulletHolder.gameObject.SetActive(false);
    }
    public override void Reselect()
    {
        _bulletHolder.gameObject.SetActive(false);
        _wp.StartCoroutine(CoolDown());
    }

    public override void ReEquip()
    {
        //_wp.StartCoroutine(CoolDown());
        //BulletHolder.gameObject.SetActive(false);
    }

    public override BaseMagazine Clone()
    {
        return new LaserHeatMagazine();
    }
    public override Settings GiveSettings()
    {
        return _settings;
    }
}


public class MeleMagazine : BaseMagazine
{
    BaseBullet _bulletHolder;
    [SerializeField] private Settings _settings;



    public override void AddData(Transform shootpivot, RangeWeapon wp, Settings settings)
    {
        _settings = settings;
        //_settings = settings as LaserHeatSettings;
        if (_bulletHolder == null)
        {
            _bulletHolder = Object.Instantiate(settings.BulletData.BulletPrefab, shootpivot);
            _bulletHolder.transform.position = shootpivot.position;
        }
        _bulletHolder.SetUp(settings.BulletData);



        base.AddData(shootpivot, wp, settings);
    }


    public override void Fire()
    {
        _bulletHolder.ExternalInput();
        _bulletHolder.gameObject.SetActive(true);
    }

    public override void FireIsUp()
    {
        _bulletHolder.gameObject.SetActive(false);
    }

    public override BaseMagazine Clone()
    {
        return new MeleMagazine();
    }
    public override Settings GiveSettings()
    {
        return _settings;
    }

}