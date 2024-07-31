using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmTestVisual : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _particle;
    [SerializeField] Vector3 _shootPoint;
    [SerializeField] float _colddown = 5;

    float _time = 0;

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _colddown)
        {
            _anim.Play("Hit");
            _time = 0;
        }
    }

    public void HitMethod()
    {
        Instantiate(_particle, transform.position + _shootPoint, Quaternion.identity);
    }
}
