using HookToolSystem;
using UnityEngine;
using UnityServiceLocator;
using WeaponSystem;

public class MaincharacterBoostraper : Bootstrapper
{
    protected override void Bootstrap()
    {
        Register(GetComponent<HookTool>());
        Register(GetComponent<WeaponControler>());
        Register(GetComponent<Rigidbody2D>());
    }
}