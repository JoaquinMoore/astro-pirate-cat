using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{

    public virtual void OpenScreen()
    {
        ChangeState(true);
    }
    public virtual void CloseScreen()
    {
        ChangeState(false);
    }


    public virtual void ChangeState(bool state) => gameObject.SetActive(state);
}
