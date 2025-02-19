using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class BaseBulletData : ScriptableObject
    {
        public BaseBullet BulletPrefab;
        public int HardPrefCap = 1000;
        public int SoftPrefCap = 100;


        [Header("Data")]
        public GameObject ParticlePref;

    }
}