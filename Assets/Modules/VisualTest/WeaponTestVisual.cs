using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTestVisual : MonoBehaviour
{
    [SerializeField] posInWar _pos;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _particle;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _colddown = 5;

    float _time = 0;
    GameObject currentBullet;

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _colddown)
        {
            Instantiate(_particle, _shootPoint.position, _shootPoint.rotation);
            currentBullet = Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation);
            currentBullet.GetComponent<Bullet>().Initial(_pos);
            _time = 0;
        }
    }
}

public enum posInWar
{
    player,
    enemy,
}