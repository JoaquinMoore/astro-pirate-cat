using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public enum Aliance
    {
        allies = 7,
        enemy = 6,
        neutral = 0

    };



    public class BaseBulletData : ScriptableObject
    {
        public BaseBullet BulletPrefab;
        public int HardPrefCap = 1000;
        public int SoftPrefCap = 100;


        [Header("Data")]
        public GameObject ParticlePref;
        [Tooltip("esto ")] public Aliance Aliance;
    }
}