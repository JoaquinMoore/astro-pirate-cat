using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : BaseBullet
{

    protected int _damage;
    protected List<string> _targetsTag;
    public override void SetUp(BaseBulletData data)
    {
        LaserBulletData _data = data as LaserBulletData;

        _damage = _data.Damage;
    }

    public override void ExternalInput()
    {
        Debug.Log("laser pew");
    }


}
