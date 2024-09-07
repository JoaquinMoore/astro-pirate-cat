using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "Laser Bullet", menuName = "Weapons/Bullets/Laser Bullet")]

    public class LazerBulletData : HitBoxBulletData
    {
        public float LaserLengh;

    }
}