using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapons/Weapons/Ranged Weapon")]
    public class RangeWeaponData : BaseWeaponData
    {
        [SerializeReference] public List<Magazine> Magazines;



        [HideInInspector] public bool SyncedSelector;



        [ContextMenu("Add Standar Magazine")]
        public void StandardMag()
        {
            Magazine hold = new()
            {
                _Magazine = new StandardMagazine() { name = "standard mag" },
                Position = Magazines.Count,
            };
            Magazines.Add(hold);
        }

        [ContextMenu("Add Laser Magazine")]
        public void LazMag()
        {
            Magazine hold = new()
            {
                _Magazine = new LaserHeatMagazine() { name = "laser mag" },
                Position = Magazines.Count,
            };
            Magazines.Add(hold);
        }

        [ContextMenu("Add Melle Magazine")]
        public void Melle()
        {
            Magazine hold = new()
            {
                _Magazine = new MeleMagazine() { name = "Melle mag" },
                Position = Magazines.Count,
            };
            Magazines.Add(hold);
        }

        [ContextMenu("Add Parabolic Magazine")]
        public void Parabolic()
        {
            Magazine hold = new()
            {
                _Magazine = new ParabolicMagazine() { name = "Parabolic mag" },
                Position = Magazines.Count,
            };
            Magazines.Add(hold);
        }
    }
    [System.Serializable]
    public class Magazine
    {
        [SerializeReference] public BaseMagazine _Magazine;
        [HideInInspector] public int Position;
    }
}