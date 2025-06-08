using Npc;
using UnityEngine;

public abstract class TaskWrapperSO : ScriptableObject
{
    public abstract ITask<NPCController> Clone();
}