using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaincharacterPlayerBrain : MonoBehaviour
{
    private void Awake()
    {
        _controller = GetComponent<MaincharacterController>();
    }

    private IMaincharacterController _controller;
}
