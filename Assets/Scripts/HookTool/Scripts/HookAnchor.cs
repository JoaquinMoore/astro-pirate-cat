using UnityEngine;
using UnityEngine.Events;

namespace HookToolSystem
{
    [DisallowMultipleComponent]
    public class HookAnchor : MonoBehaviour
    {
        public UnityEvent OnHook;
        public UnityEvent OnRealese;

        public enum AnchorType
        {
            Swing,
            Approach
        }

        public AnchorType typeOfAnchor;
    }
}
