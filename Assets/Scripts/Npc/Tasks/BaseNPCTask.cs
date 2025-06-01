using System;

namespace Npc.Tasks
{
    [Serializable]
    public abstract class BaseNPCTask
    {
        public abstract void Execute(NPCFacade npc);

        public abstract string Log();
    }
}