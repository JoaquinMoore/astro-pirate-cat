using HookToolSystem;
using UnityEngine;

public class MainCharacterController : MonoBehaviour, IMainCharacterController
{
    public void Hook(Collider2D collider, GameObject hook)
    {
        _hookTool.Grab(collider, hook);
    }

    public void Jump()
    {
        Debug.Log("Jump");
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
    }

    private HookTool _hookTool;
}
