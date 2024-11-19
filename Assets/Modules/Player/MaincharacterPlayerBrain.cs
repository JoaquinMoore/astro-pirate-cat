using UnityEngine;

public class MainCharacterPlayerBrain : IBrain<MainCharacterController>
{
    public void Awake(MainCharacterController controller)
    {
        _controller = controller;
    }

    public void Think()
    {
        Debug.Log("El jugador esta pensando");
    }

    private MainCharacterController _controller;
}
