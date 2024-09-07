using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipVIsualTest : MonoBehaviour
{
    [SerializeField] Slider _sliderLife;
    [SerializeField] Animator _anim;
    [SerializeField] float _damage;
    [SerializeField] float _currentLife;
    [SerializeField] float _maxLife;

    private void Awake()
    {
        _anim.SetFloat("Blend", _currentLife / _maxLife);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentLife -= _damage;
            _sliderLife.value = _currentLife / _maxLife;
            _anim.SetFloat("Blend", _currentLife / _maxLife);
        }
    }
}
