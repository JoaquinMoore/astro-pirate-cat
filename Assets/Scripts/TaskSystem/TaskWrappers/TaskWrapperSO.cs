using Npc;
using UnityEngine;

namespace TaskSystem.TaskWrappers
{
    public abstract class TaskWrapperSO : ScriptableObject
    {
        public abstract ITask<NPCController> Clone();
    }
}