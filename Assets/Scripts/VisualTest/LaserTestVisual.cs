using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTestVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRend;
    [SerializeField] Animator _anim;
    [SerializeField] float _timeUsed = 10f;
    [SerializeField] float _coldDown = 5f;
    [Header("Bullet")]
    [SerializeField] Transform _initialPoint;
    [SerializeField] Transform _headPoint;
    [SerializeField] LineRenderer _bulletPlayerPrefab;

    float _time;
    bool _stateColor;

    private void Awake()
    {
        _bulletPlayerPrefab.SetPosition(0, _initialPoint.position);
        _bulletPlayerPrefab.SetPosition(1, _headPoint.position);
    }

    private void Update()
    {
        _time += Time.deltaTime / _timeUsed;

        if (_time < 1)
        {
            VisualLaserChanged(_stateColor);
        }
        else
        {
            _anim.SetTrigger("IsActive");
            _stateColor = !_stateColor;
            _time = 0;
        }
    }

    void VisualLaserChanged(bool isRed)
    {
        if (isRed)
        {
            Debug.Log("Rojo");
            _spriteRend.color = Color.Lerp(Color.red, Color.white, _time);

            if (_bulletPlayerPrefab.gameObject.activeSelf)
            {
                _bulletPlayerPrefab.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Blanco");
            _spriteRend.color = Color.Lerp(Color.white, Color.red, _time);

            if (!_bulletPlayerPrefab.gameObject.activeSelf)
            {
                _bulletPlayerPrefab.gameObject.SetActive(true);
            }
        }
    }
}
