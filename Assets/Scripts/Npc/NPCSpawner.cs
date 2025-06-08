using UnityEngine;

namespace Npc
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeReference]
        private NPCController _npcRef;

        [SerializeField]
        private TaskWrapperSO _task;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var npc = Instantiate(_npcRef);
            npc.DefaultTask = _task.Clone();
        }
    }
}