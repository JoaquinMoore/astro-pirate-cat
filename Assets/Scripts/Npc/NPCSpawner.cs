using Npc.Tasks;
using UnityEngine;

namespace Npc
{
    public class NPCSpawner : MonoBehaviour
    {
        public NPCFacade npcRef;

        [SerializeReference]
        public BaseTaskWrapper task;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var npc = Instantiate(npcRef);
            npc.AddTask(task.GetTask());
        }

        [ContextMenu("Create Interact Task")]
        public void CreateInteractTask()
        {
            task = new TaskInteractWrapper();
        }

        [ContextMenu("Create Move Task")]
        public void CreateMoveTask()
        {
            task = new TaskMoveWrapper();
        }
    }
}