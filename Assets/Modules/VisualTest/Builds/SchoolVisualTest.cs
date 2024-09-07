using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolVisualTest : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] string _type;
    [SerializeField] string _playOpenName;

    void Awake()
    {
        _anim.SetTrigger(_type);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.SetBool(_playOpenName, !_anim.GetBool(_playOpenName));
        }
    }
}
