using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace WeaponSystem
{
    public class RangeWeapon : BaseWeapon<RangeWeaponData>
    {
        [SerializeField] private Transform _shootPivot;
        [SerializeField] private GameObject _sprite;

        [SerializeReference] private BaseSelector _selector;
        [SerializeReference] private BaseMagazine _currentMag;
        BaseTrigger _currentTrigger;


        float _currentSpread;

        float _timeInterbalFirerate = 60f;

        public override void SetUpData(RangeWeaponData weaponData)
        {

            List<BaseTrigger> triggers = new();
            List<BaseMagazine> mags = new();

            foreach (var item in weaponData.Triggers)
            {
                BaseTrigger hold1 = item._Trigger.Clone();
                hold1.AddData(_anims, item._Trigger.GiveSettings());
                triggers.Add(hold1);
            }
            foreach (var item in weaponData.Magazines)
            {
                BaseMagazine hold2 = item._Magazine.Clone();
                hold2.AddData(_shootPivot, this, item._Magazine.GiveSettings());
                mags.Add(hold2);
            }
            if (weaponData.SyncedSelector)
            {

            }
            else
            {
                _selector = new StandarSelector(triggers, mags);
            }

            _currentTrigger = triggers[0];
            _currentMag = mags[0];
            Reflesh();

        }

        #region ControlerInputs
        public override void PrimaryFireIsDown()
        {
            if (_anims.GetFloat("FireRate") != (_currentTrigger._FireRate / _timeInterbalFirerate))
                Reflesh();
            if (!_currentTrigger.CanFire())
            {
                CantFire();
                return;
            }

            if (!_currentMag.CanFire())
            {
                CantFire();
                return;
            }

            _onFire = true;
            Fire();
        }

        public override void PrimaryFireWasUp()
        {

            _currentSpread = 0;
            _onFire = false;
        }

        public override void ChangeFireMode()
        {
            _currentTrigger.Reset();
            _currentTrigger = _selector.ChangeFireType();

            Reflesh();
        }
        public override void ChangeMagMode()
        {
            _currentMag.Reset();
            _currentMag = _selector.ChangeMagazineType();
            _currentMag.Reselect();
        }
        #endregion

        #region WeaponFuncs
        public void CantFire()
        {
            _currentMag.FireIsUp();
            _currentTrigger.FireWasUp();
        }

        public void Fire()
        {
            _anims.SetTrigger("Fire");
        }
        public override void AnimFire()
        {
            _currentTrigger.FireIsDown();
            _currentMag.Fire();
            OnImpulseAction.Invoke(-transform.right * _currentMag.RecoilReturn());
            Spread();
        }

        public override void EndAnimFire()
        {
            if (_onFire)
                return;

            _anims.ResetTrigger("Fire");
            _currentTrigger.FireWasUp();
            _currentMag.FireIsUp();
        }

        public void Spread()
        {
            _currentSpread = _currentMag.SpreadMaker();
        }

        public override float WeaponSpread()
        {
            return _currentSpread;
        }

        public override void Reflesh()
        {
            _anims.SetFloat("FireRate", (_currentTrigger._FireRate / _timeInterbalFirerate));
            _currentMag.ReEquip();

        }
        #endregion

        #region Selection
        public override void Select()
        {
            _sprite.SetActive(true);
            if (_sprite != null)
                _sprite.SetActive(true);
            _anims.enabled = true;
            _currentTrigger.FireWasUp();
        }
        public override void Deselect()
        {
            _sprite.SetActive(false);
            if (_sprite != null)
                _sprite.SetActive(false);
            _currentMag.Reset();
            _currentTrigger.Reset();
            _anims.enabled = false;
            _currentTrigger.FireWasUp();
            _currentMag.FireIsUp();
        }
        #endregion




    }
}