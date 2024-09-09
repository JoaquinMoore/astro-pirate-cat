using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class WeaponControler : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Transform _WeaponSpawner;
        [SerializeField] private Transform _WeaponRot;
        [SerializeField] private List<BaseWeaponData> _Data;

        [Header("Debug")]
        public bool TestingInputs;
        [SerializeField] private List<WeaponSlot> _weapons;
        [SerializeField] private int _weaponInt;

        [SerializeField] private WeaponSlot _currentWeapon;
        [SerializeField] private WeaponSlot _selectedWeapon;

        [SerializeField] private SpriteRenderer _ArmRef;

        public event Action<Vector2> OnImpulse = delegate { };

        [SerializeField] private bool _firing;
        void Start()
        {
            _ArmRef = _WeaponRot.GetComponentInChildren<SpriteRenderer>();


            foreach (var item in _Data)
            {
                AddWeapons(item);
            }


            _weapons[0].Weapon.Select();
            _currentWeapon = _weapons[0];
            _weapons[0].WeaponEquiped = true;
            if (_weapons.Count > 1)
                _selectedWeapon = _weapons[1];

            if (_ArmRef != null)
                _ArmRef.enabled = !_currentWeapon.HideArmOnEquiped;
        }


        void Update()
        {



            if (TestingInputs)
            {
                TestingInput();
            }
        }

        public void TestingInput()
        {
            MouseAim(Camera.main.ScreenToWorldPoint(Input.mousePosition));



            if (Input.GetKey(KeyCode.Mouse0))
            {
                PrimaryFireDown();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                PrimaryFireUp();
            }


            if (Input.GetKeyDown(KeyCode.V))
            {
                _currentWeapon.Weapon.ChangeFireMode();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                _currentWeapon.Weapon.ChangeMagMode();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwapPrimaryWeapon();
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                ChangeSecondaryWeapon(Input.mouseScrollDelta.y);
            }
        }

        #region Inputs
        public void PrimaryFireDown()
        {
            _currentWeapon.Weapon.PrimaryFireIsDown();
            _firing = true;
        }

        public void PrimaryFireUp()
        {
            _currentWeapon.Weapon.PrimaryFireWasUp();
            _firing = false;
        }

        public void ChangeFireMode()
        {
            _currentWeapon.Weapon.ChangeFireMode();
        }

        public void ChangePrimaryWeapon()
        {
            SwapPrimaryWeapon();
        }


        public void MouseAim(Vector2 target)
        {

            Vector2 pos = target - (Vector2)transform.position;
            _WeaponRot.transform.right = pos + new Vector2(0, _currentWeapon.Weapon.WeaponSpread());


            //if (pos.x > 0)
            //    CurrentWeapon.weaponSpriteRenderer.flipY = false;
            //else
            //    CurrentWeapon.weaponSpriteRenderer.flipY = true;

        }


        public void ChangeSecondaryWeapon(float direction)
        {

            _weaponInt += (int)direction;
            _weaponInt = (int)Mathf.Repeat(_weaponInt, _weapons.Count);

            if (_weapons[_weaponInt].WeaponEquiped && _weapons.Count > 2)
                _weaponInt += (int)direction;

            if (_weaponInt < 0)
            {
                _weaponInt = (int)Mathf.Repeat(_weaponInt, _weapons.Count);
                return;
            }
            _selectedWeapon = _weapons[_weaponInt];

        }






        public void SwapPrimaryWeapon()
        {
            if (_weapons.Count == 1)
                return;

            StopAllCoroutines();
            WeaponSlot current = _currentWeapon;
            WeaponSlot selected = _selectedWeapon;

            if (_ArmRef != null)
                _ArmRef.enabled = !selected.HideArmOnEquiped;
            Debug.Log(selected.HideArmOnEquiped);

            current.Weapon.PrimaryFireWasUp();
            current.Weapon.Deselect();
            selected.Weapon.Select();
            _currentWeapon.WeaponEquiped = false;
            _selectedWeapon.WeaponEquiped = true;
            _currentWeapon = selected;
            _selectedWeapon = current;
            _currentWeapon.Weapon.Reflesh();
            _selectedWeapon.Weapon.Reflesh();
        }

        #endregion


        public void AddWeapons(BaseWeaponData data)
        {
            BaseWeapon wep = Instantiate(data.WeaponPrefab, _WeaponSpawner);
            wep.AddData(data, this);
            wep.Deselect();
            WeaponSlot hol = new();
            hol.Weapon = wep;
            wep.OnImpulseAction += impulse => OnImpulse(impulse);
            hol.HideArmOnEquiped = data.HideArm;

            _weapons.Add(hol);
        }



        [System.Serializable]
        public class WeaponSlot
        {
            public BaseWeapon Weapon;
            public bool WeaponEquiped;
            public bool HideArmOnEquiped;
        }
    }
}