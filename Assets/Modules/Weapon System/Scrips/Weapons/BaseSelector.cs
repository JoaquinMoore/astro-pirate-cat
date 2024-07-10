using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSelector
{


    
    public virtual BaseTrigger ChangeFireType() { return default; }
    public virtual BaseMagazine ChangeMagazineType() { return default; }

}
[System.Serializable]
public class StandarSelector : BaseSelector
{
    private List<BaseTrigger> _Triggers = new();
    private List<BaseMagazine> _Maggazines = new();

    int CurrentFireMode;
    int CurrentMagMode;
    public StandarSelector(List<BaseTrigger> Triggers, List<BaseMagazine> Maggazines)
    {
        _Triggers = Triggers;
        _Maggazines = Maggazines;
    }

    public override BaseTrigger ChangeFireType() 
    {
        CurrentFireMode++;
        if (CurrentFireMode >= _Triggers.Count)
            CurrentFireMode = 0;

        return _Triggers[CurrentFireMode];
    }

    public override BaseMagazine ChangeMagazineType() 
    {
        CurrentMagMode++;
        if (CurrentMagMode >= _Maggazines.Count)
            CurrentMagMode = 0;

        return _Maggazines[CurrentMagMode]; 
    }
}