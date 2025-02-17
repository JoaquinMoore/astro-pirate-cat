using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildSystem;
public class testbuildnpc : MonoBehaviour
{
    public BuildingBase building;
    public GameObject pref;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            building.SelectTask(Instantiate(pref));
        }
    }
}
