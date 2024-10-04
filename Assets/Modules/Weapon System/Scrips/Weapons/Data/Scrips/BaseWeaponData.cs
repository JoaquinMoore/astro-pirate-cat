using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class BaseWeaponData : ScriptableObject
    {
        public BaseWeapon WeaponPrefab;

        [SerializeReference] public List<Trigger> Triggers;
        public bool HideArm;


        [ContextMenu("Add SemiFire")]
        public void SemiAuto()
        {
            Trigger hold = new()
            {
                _Trigger = new SemiTrigger() { name = "semi" },
                Position = Triggers.Count,
            };
            Triggers.Add(hold);
            Debug.Log(Triggers[hold.Position]._Trigger);
        }

        [ContextMenu("Add FullFire")]
        public void FullAuto()
        {
            Trigger hold = new()
            {
                _Trigger = new FullTrigger() { name = "full" },
                Position = Triggers.Count,
            };
            Triggers.Add(hold);
            Debug.Log(Triggers[hold.Position]._Trigger);
        }

    }
    [System.Serializable]
    public class Trigger
    {
        [SerializeReference] public BaseTrigger _Trigger;
        public int Position;
    }
}