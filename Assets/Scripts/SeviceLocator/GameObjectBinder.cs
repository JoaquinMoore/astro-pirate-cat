using UnityEngine;
using UnityServiceLocator;

public class GameObjectBinder : MonoBehaviour
{
    [SerializeField]
    private Component[] _components;

    private void Awake()
    {
        foreach (var component in _components)
        {
            ServiceLocator.For(gameObject).Register(component.GetType(), component);
        }
    }
}