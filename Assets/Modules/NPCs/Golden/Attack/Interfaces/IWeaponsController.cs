using UnityEngine;

namespace Assets.Modules.NPCs.Golden.Attack.Interfaces
{
    public interface IWeaponsController
    {
        public void SelectRangeWeapon();
        public void SelectMeleeWeapon();
        public void SetTarget(GameObject target);
        public void Attack();
    }
}
