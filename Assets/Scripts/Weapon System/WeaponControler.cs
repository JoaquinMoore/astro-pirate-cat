using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControler : MonoBehaviour
{
    [Header("Referencias")]
    public Transform _WeaponHolder;
    public List<WeaponDataBase> Data;

    [Header("Debug")]
    public WeaponBase weapon;

    void Start()
    {
        
    }


    void Update()
    {
        MouseAim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }


    public void MouseAim(Vector2 target)
    {

        Vector2 pos = target - (Vector2)transform.position;
        _WeaponHolder.transform.right = pos;


        //if (pos.x > 0)
        //    CurrentWeapon.weaponSpriteRenderer.flipY = false;
        //else
        //    CurrentWeapon.weaponSpriteRenderer.flipY = true;

    }
}
