using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : WeaponBase<RangeWeaponData>
{
    public List<BaseMagazine> Mags;

    BaseMagazine CurrentMag;
    BaseTrigger CurrentTrigger;

    int CurrentFireMode;
    int CurrentMagMode;

    public override void SetUpData(RangeWeaponData weaponData)
    {

    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void PrimaryFireIsDown()
    {
        if (!CurrentTrigger.CanFire())
            return;
        AnimFire();
    }

    public override void PrimaryFireWasUp()
    {
        base.PrimaryFireWasUp();
    }

    public override void ChangeFireMode()
    {
        base.ChangeFireMode();
    }
    public override void AnimFire()
    {
        base.AnimFire();
    }
    public void Fire()
    {
        CurrentTrigger.FireIsDown();
        CurrentMag.Fire();
    }
}
