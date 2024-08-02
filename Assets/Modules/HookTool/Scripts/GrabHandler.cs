using UnityEngine;

namespace Core.HookTool
{

    [RequireComponent(typeof(HookTool))]
    public class GrabHandler : MonoBehaviour
    {
        private HookTool _tool;

        private void Awake()
        {
            _tool = GetComponent<HookTool>();
        }
    }
}
