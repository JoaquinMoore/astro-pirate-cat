using System;
using _UTILITY.PoolManager;
using Npc;
using Npc.Tasks;

public class TaskMove : BaseNPCTask, IPoolable<TaskMove>
{
    public event Action<TaskMove> OnRelease;

    public override void Execute(NPCFacade npc)
    {
    }
}