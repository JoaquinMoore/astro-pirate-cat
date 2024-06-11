using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public virtual WeaponDataBase Data { get; }

    public List<BaseTrigger> Trigger = new();

    public virtual void AddData(WeaponDataBase weaponData, WeaponControler father) { }

    public virtual void PrimaryFireIsDown() { }
    public virtual void PrimaryFireWasUp() { }
    public virtual void ChangeFireMode() { }
    public virtual void AnimFire() { }
}

public class WeaponBase<T> : WeaponBase where T : WeaponDataBase
{
    protected WeaponControler _father;
    protected T data;
    public override WeaponDataBase Data => data;
    public override void AddData(WeaponDataBase weaponData, WeaponControler father) { data = weaponData as T; _father = father; SetUpData(data); }
    public virtual void SetUpData(T weaponData) { }

    public virtual void ParticleHandler() { }
}