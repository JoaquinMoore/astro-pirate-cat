using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : BaseWeapon<RangeWeaponData>
{
    [SerializeField] private Transform _shootPivot;
    [SerializeField] private GameObject _sprite;

    [SerializeReference] private BaseSelector _selector;
    [SerializeReference] private BaseMagazine _currentMag;
    BaseTrigger _currentTrigger;

    float _currentSpread;

    float _timeInterbalFirerate = 60f;

    public override void SetUpData(RangeWeaponData weaponData)
    {
        List<BaseTrigger> triggers = new();
        List<BaseMagazine> mags = new();

        foreach (var item in weaponData.Triggers)
        {
            BaseTrigger hold1 = item._Trigger.Clone();
            hold1.AddData(_anims, item._Trigger.GiveSettings());
            triggers.Add(hold1);
        }
        foreach (var item in weaponData.Magazines)
        {
            BaseMagazine hold2 = item._Magazine.Clone();
            hold2.AddData(_shootPivot, this, item._Magazine.GiveSettings());
            mags.Add(hold2);
        }
        if (weaponData.SyncedSelector)
        {

        }
        else
        {
            _selector = new StandarSelector(triggers, mags);
        }

        _currentTrigger = triggers[0];
        _currentMag = mags[0];
        Reflesh();

    }


    void Start()
    {

    }


    void Update()
    {
        
    }

    public override void PrimaryFireIsDown()
    {
        if (_anims.GetFloat("FireRate") != (_currentTrigger._FireRate / _timeInterbalFirerate))
            Reflesh();
        if (!_currentTrigger.CanFire())
        {
            CantFire();
            return;
        }

        if (!_currentMag.CanFire())
        {
            CantFire();
            return;
        }


        Fire();
    }

    public override void PrimaryFireWasUp()
    {
        _anims.ResetTrigger("Fire");
        _currentTrigger.FireWasUp();
        _currentMag.FireIsUp();
        _currentSpread = 0;
    }

    public override void ChangeFireMode()
    {
        _currentTrigger.Reset();
        _currentTrigger = _selector.ChangeFireType();

        Reflesh();
    }
    public override void ChangeMagMode()
    {
        _currentMag.Reset();
        _currentMag = _selector.ChangeMagazineType();
        _currentMag.Reselect();
    }

    public void CantFire()
    {
        _currentMag.FireIsUp();
        _currentTrigger.FireWasUp();
    }

    public void Fire()
    {
        _anims.SetTrigger("Fire");
    }
    public override void AnimFire()
    {
        _currentTrigger.FireIsDown();
        _currentMag.Fire();
        OnImpulseAction.Invoke(-transform.right * _currentMag.RecoilReturn());
        Spread();
    }

    public void Spread()
    {
        _currentSpread = _currentMag.SpreadMaker();
    }

    public override float WeaponSpread()
    {
        return _currentSpread;
    }

    public override void Reflesh()
    {
        _anims.SetFloat("FireRate", (_currentTrigger._FireRate / _timeInterbalFirerate));
        _currentMag.ReEquip();
    }


    public override void Select()
    {
        _sprite.SetActive(true);
    }
    public override void Deselect()
    {
        _sprite.SetActive(false);
        _currentMag.Reset();
    }
}
