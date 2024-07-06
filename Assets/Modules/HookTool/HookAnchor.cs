using UnityEngine;

namespace HookTool
{
    [DisallowMultipleComponent]
    public class HookAnchor : MonoBehaviour
    {
        public enum AnchorType
        {
            Swing,
            Approach
        }

        public AnchorType typeOfAnchor;
    }
}
