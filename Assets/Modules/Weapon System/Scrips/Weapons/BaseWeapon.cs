using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class BaseWeapon : MonoBehaviour
    {
        public Action<Vector2> OnImpulseAction;

        protected virtual BaseWeaponData _data { get; }

        public Animator _anims;

        public virtual void AddData(BaseWeaponData weaponData, WeaponControler father) { }

        public virtual void PrimaryFireIsDown() { }
        public virtual void PrimaryFireWasUp() { }
        public virtual void ChangeFireMode() { }
        public virtual void ChangeMagMode() { }
        public virtual float WeaponSpread() { return default; }
        public virtual void AnimFire() { }
        public virtual void Reflesh() { }
        public virtual void Select() { }
        public virtual void Deselect() { }
    }

    public class BaseWeapon<T> : BaseWeapon where T : BaseWeaponData
    {
        protected WeaponControler _father;
        protected T data;
        protected override BaseWeaponData _data => data;
        public override void AddData(BaseWeaponData weaponData, WeaponControler father) { data = weaponData as T; _father = father; SetUpData(data); }
        public virtual void SetUpData(T weaponData) { }

        public virtual void ParticleHandler() { }
    }
}