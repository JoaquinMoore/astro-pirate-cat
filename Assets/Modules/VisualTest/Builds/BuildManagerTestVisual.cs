using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManagerTestVisual : MonoBehaviour
{
    [SerializeField] GameObject[] _wall;
    Camera _cam;
    
    void Start()
    {
        _cam = Camera.main;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var wall = Instantiate(_wall[Random.Range(0, _wall.Length)], (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            wall.GetComponent<Animator>().Play("Instance");
        }
    }
}
