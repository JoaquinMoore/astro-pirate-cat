using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger
{
    [field: SerializeField] public int FireRate { get; private set; } = 400;
    protected bool Canfire = true;
    protected Animator _anim;
    public virtual void AddData(Animator anim) { _anim = anim; }
    public virtual void FireIsDown() { }
    public virtual void FireWasUp() { }
    public virtual void FireRequirement() { }
    public virtual bool CanFire() { return Canfire; }
}

public class SemiTrigger : BaseTrigger
{
    public override void FireIsDown()
    {
        Canfire = false;
        _anim.SetBool("Active", Canfire);
    }
    public override void FireWasUp()
    {
        Canfire = true;
        _anim.SetBool("Active", Canfire);
    }

}

public class FullTrigger : BaseTrigger
{

    public override void FireWasUp()
    {
        Canfire = false;
        _anim.SetBool("Active", Canfire);
    }

    public override bool CanFire()
    {
        Canfire = true;
        _anim.SetBool("Active", Canfire);
        return base.CanFire();
    }
}