using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildSystem;

public class testrepair : MonoBehaviour
{
    public GameObject testobj;
    public BuildingBase testbuild;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            testbuild.SelectTask(Instantiate(testobj));
        }
    }
}
