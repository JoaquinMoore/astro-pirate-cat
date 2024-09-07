using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVisualTest : MonoBehaviour
{
    [SerializeField] Animator _portalAnim;

    int numTest = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (numTest == 0)
                _portalAnim.SetTrigger("Open");
            else if (numTest == 1)
                _portalAnim.SetTrigger("Close");
            numTest++;

            if (numTest >= 2)
                numTest = 0;
        }
    }
}
