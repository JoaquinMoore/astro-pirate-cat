using Npc;

namespace TaskSystem.Tasks
{
    public class TaskAttackShip : ITask<NPCController>
    {
        public bool IsComplete { get; private set; }
        bool ITask<NPCController>.HasStarted { get; set; }

        void ITask<NPCController>.Start(NPCController context)
        {
            context.Target = Ship.Instance.gameObject;
            IsComplete = true;
        }

        void ITask<NPCController>.Execute(NPCController context)
        { }

        public ITask<NPCController> Clone() => new TaskAttackShip();
    }
}