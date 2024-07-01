using UnityEngine;

[DisallowMultipleComponent]
public class HookAnchor : MonoBehaviour
{
    public enum AnchorType
    {
        Swing,
        Approach
    }

    [field: SerializeField]
    public AnchorType TypeOfAnchor { get; private set; }
}
