using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelderTestVisual : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _particle;
    [SerializeField] Transform _shootPoint;

    GameObject _currentParticle;

    private void Start()
    {
        _currentParticle = Instantiate(_particle, _shootPoint.position, _shootPoint.rotation);
        _currentParticle.transform.SetParent(transform);
        _anim.SetBool("Mine", true);
    }
}
