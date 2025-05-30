using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelTestVisual : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _content;

    bool _isPressed;
    bool _isActive;

    //Activa el boton del panel de info
    public void Button_Click()
    {
        _isActive = !_isActive;

        _anim.SetBool("IsActive", _isActive);
        _content.SetActive(_isActive);
    }

    //Activa el boton del panel de info
    public void Button_ClickEnter()
    {
        _isPressed = true;

        _anim.SetBool("IsPressed", _isPressed);

        Debug.Log("Entre");
    }

    //Activa el boton del panel de info
    public void Button_ClickExit()
    {
        _isPressed = false;

        _anim.SetBool("IsPressed", _isPressed);

        Debug.Log("Sali");
    }
}
