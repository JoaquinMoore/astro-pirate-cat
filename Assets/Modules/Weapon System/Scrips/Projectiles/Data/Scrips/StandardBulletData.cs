using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "Standar Bullet", menuName = "Weapons/Bullets/Standar Bullet")]
    public class StandardBulletData : BaseBulletData
    {

        public int _damage;
        public float _speed;
        public float _timer;

        [Header("arte")]
        public Material AlainceMat;
        public GameObject AlainceParticle;

    }
}