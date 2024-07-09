using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser Bullet", menuName = "Weapons/Bullets/Laser Bullet")]
public class LaserBulletData : BaseBulletData
{
    public int Damage;
    public int MaxTargets;
}
