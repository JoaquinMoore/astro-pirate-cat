using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform _WeaponHolder;
    [SerializeField] private List<BaseWeaponData> _Data;

    [Header("Debug")]
    public bool TestingInputs;
    [SerializeField] private List<WeaponSlot> _weapons;
    [SerializeField] private int _weaponInt;

    [SerializeField] private WeaponSlot _currentWeapon;
    [SerializeField] private WeaponSlot _selectedWeapon;

    public event Action<Vector2> OnImpulse = delegate { };

    [SerializeField] private bool _firing;
    void Start()
    {
        foreach (var item in _Data)
        {
            AddWeapons(item);
        }
        
        _weapons[0].Weapon.Select();
        _currentWeapon = _weapons[0];
        _weapons[0].WeaponEquiped = true;
        _selectedWeapon = _weapons[1];
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
        _WeaponHolder.transform.right = pos + new Vector2(0, _currentWeapon.Weapon.WeaponSpread());


        //if (pos.x > 0)
        //    CurrentWeapon.weaponSpriteRenderer.flipY = false;
        //else
        //    CurrentWeapon.weaponSpriteRenderer.flipY = true;

    }


    public void ChangeSecondaryWeapon(float direction)
    {

        _weaponInt += (int)direction;
        _weaponInt = (int)Mathf.Repeat(_weaponInt, _weapons.Count);

        if (_weapons[_weaponInt].WeaponEquiped)
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
        StopAllCoroutines();
        WeaponSlot current = _currentWeapon;
        WeaponSlot selected = _selectedWeapon;

        
        current.Weapon.PrimaryFireWasUp();
        current.Weapon.Deselect();
        selected.Weapon.Select();
        _currentWeapon.WeaponEquiped = false;
        _selectedWeapon.WeaponEquiped = true;
        _currentWeapon = selected;
        _selectedWeapon = current;
        _currentWeapon.Weapon.Reflesh();
        _selectedWeapon.Weapon.Reflesh();
        StartCoroutine(CheckNonActiveWeapon());
    }

    //temp hasta que se me ocurra algo mejor
    public IEnumerator CheckNonActiveWeapon()
    {

        yield return new WaitForSeconds(0.1f);
        Debug.Log("test");
        _selectedWeapon.Weapon.Deselect();
        _selectedWeapon.Weapon.PrimaryFireWasUp();
        CheckFiring();
    }

    public void CheckFiring()
    {
        if (_firing == true)
            return;
        _currentWeapon.Weapon.PrimaryFireWasUp();
    }

    #endregion


    public void AddWeapons(BaseWeaponData data)
    {
        BaseWeapon wep = Instantiate(data.WeaponPrefab, _WeaponHolder);
        wep.AddData(data, this);
        wep.Deselect();
        WeaponSlot hol = new();
        hol.Weapon = wep;
        wep.OnImpulseAction += impulse => OnImpulse(impulse);

        _weapons.Add(hol);
    }



    [System.Serializable]
    public class WeaponSlot
    {
        public BaseWeapon Weapon;
        public bool WeaponEquiped;
    }
}
