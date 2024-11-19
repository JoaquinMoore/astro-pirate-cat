using UnityServiceLocator;
using UnityEngine;
using HookToolSystem;

[RequireComponent(typeof(HookTool), typeof(Rigidbody2D))]
public class MainCharacterController : MonoBehaviour
{
    /// <summary>
    /// Aplica un impulso personaje en una direccion.
    /// </summary>
    /// <param name="force">Impulso a aplicar.</param>
    public void Impulse(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Indica donde debe engancharse.
    /// </summary>
    /// <param name="target">Objetivo al cual engancharse.</param>
    /// <param name="hook">Objeto que sirvi� como hook.</param>
    public void Hook(Collider2D target, GameObject hook)
    {
        _hookTool.Grab(target, hook);
    }

    private void Awake()
    {
        _hookTool = GetComponent<HookTool>();
        _rigidBody = GetComponent<Rigidbody2D>();
        ServiceLocator.For(this).TryGet(out _brain);
    }

    private void Update()
    {
        _brain.Think();
    }

    private HookTool _hookTool;
    private Rigidbody2D _rigidBody;
    private IBrain<MainCharacterController> _brain;
}
