using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : WeaponBase<RangeWeaponData>
{
    public List<BaseMagazine> Mags;
    public Transform _ShootPivot;
    public BaseBullet temporalbullet;
    BaseMagazine CurrentMag;
    BaseTrigger CurrentTrigger;

    int CurrentFireMode;
    int CurrentMagMode;

    public override void SetUpData(RangeWeaponData weaponData)
    {

    }


    void Start()
    {
        CurrentTrigger = new FullTrigger();
        CurrentMag = new StandardMagazine();

        CurrentTrigger.AddData(_anims);
        CurrentMag.AddData(_ShootPivot, this);
        CurrentMag.TemporalAddBullet(temporalbullet);

        _anims.SetFloat("FireRate", (CurrentTrigger.FireRate / 60f));
    }


    void Update()
    {
        
    }

    public override void PrimaryFireIsDown()
    {
        CurrentTrigger.FireRequirement();
        if (!CurrentTrigger.CanFire() && !CurrentMag.CanFire())
            return;

        Fire();
    }

    public override void PrimaryFireWasUp()
    {
        _anims.ResetTrigger("Fire");
        CurrentTrigger.FireWasUp();
    }

    public override void ChangeFireMode()
    {
        _anims.SetFloat("FireRate", (CurrentTrigger.FireRate / 60f));
    }
    public void Fire()
    {

        _anims.SetTrigger("Fire");
    }
    public override void AnimFire()
    {
        CurrentTrigger.FireIsDown();
        CurrentMag.Fire();
    }

}
