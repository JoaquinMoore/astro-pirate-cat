using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "HitBox Bullet", menuName = "Weapons/Bullets/HitBox Bullet")]
    public class HitBoxBulletData : BaseBulletData
    {
        public int Damage;
        public int MaxTargets;

    }
}