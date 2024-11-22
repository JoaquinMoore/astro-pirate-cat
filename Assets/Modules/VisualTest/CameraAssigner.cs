using UnityEngine;
using UnityEngine.UI;

public class CameraAssigner : MonoBehaviour
{
    [SerializeField] Canvas _canvas;

    private void Awake()
    {
        _canvas.worldCamera = Camera.main;
    }
}
