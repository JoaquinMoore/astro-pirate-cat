using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger
{
    public virtual void FireIsDown() { }
    public virtual void FireWasUp() { }
    public virtual bool CanFire() { return true; }
}
