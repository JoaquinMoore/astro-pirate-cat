using System;
using UnityEngine;

namespace WeaponSystem
{
    public class BaseWeapon : MonoBehaviour
    {
        public Action<Vector2> OnImpulseAction;

        protected float _softSpeedCap;

        public float SoftSpeedCap => _softSpeedCap;

        protected virtual BaseWeaponData _data { get; }

        public Animator _anims;

        protected bool _onFire;
        public virtual void AddData(BaseWeaponData weaponData, WeaponControler father) { }

        public virtual void PrimaryFireIsDown() { }
        public virtual void PrimaryFireWasUp() { }
        public virtual void ChangeFireMode() { }
        public virtual void ChangeMagMode() { }
        public virtual float WeaponSpread() { return default; }
        public virtual void AnimFire() { }
        public virtual void EndAnimFire() { }
        public virtual void Reflesh() { }
        public virtual void Select() { }
        public virtual void Deselect() { }
        public virtual void MousePos(Vector3 pos) { }
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