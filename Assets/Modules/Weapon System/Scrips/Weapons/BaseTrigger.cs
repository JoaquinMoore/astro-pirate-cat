using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTrigger
{
    public string name = "default";
    public int _FireRate { get; private set; } = 0;

    protected bool Canfire = true;
    protected Animator _anim;
    public virtual void AddData(Animator anim, Settings settings) { _anim = anim; _FireRate = settings.FireRate; }
    public virtual void FireIsDown() { }
    public virtual void FireWasUp() { }
    public virtual void Reset () { Canfire = false; _anim.SetBool("Active", Canfire); }
    public virtual bool CanFire() { return Canfire; }

    public virtual BaseTrigger Clone()
    {
        return default;
    }

    public virtual Settings GiveSettings() { return default; }

    [System.Serializable]
    public class Settings
    {
        public int FireRate;
    }
}
[System.Serializable]
public class SemiTrigger : BaseTrigger
{
    public Settings TriggerSettings;
    bool TriggerHeld = false;

    public override void FireIsDown()
    {
        Canfire = true;
        TriggerHeld = true;
        _anim.SetBool("Active", Canfire);
    }
    public override void FireWasUp()
    {
        Canfire = false;
        TriggerHeld = false;
        _anim.SetBool("Active", Canfire);
    }
    public override bool CanFire()
    {
        if (TriggerHeld)
        {
            Debug.Log("no pew");
            Canfire = false;

        }
        else
        {
            Debug.Log("pew");
            Canfire = true;
        }

        _anim.SetBool("Active", Canfire);
        return Canfire;
    }

    public override void Reset()
    {
        Canfire = true;
        TriggerHeld = false;
        _anim.SetBool("Active", Canfire);
    }

    public override BaseTrigger Clone()
    {
        return new SemiTrigger();
    }
    public override Settings GiveSettings()
    {
        return TriggerSettings;
    }
}

public class FullTrigger : BaseTrigger
{
    public Settings TriggerSettings;
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

    public override void Reset()
    {
        Canfire = true;
        _anim.SetBool("Active", Canfire);
    }

    public override BaseTrigger Clone()
    {
        return new FullTrigger();
    }

    public override Settings GiveSettings()
    {
        return TriggerSettings;
    }
}