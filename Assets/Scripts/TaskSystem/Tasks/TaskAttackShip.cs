using Npc;

namespace TaskSystem.Tasks
{
    public class TaskAttackShip : ITask<NPCController>
    {
        public bool IsComplete { get; private set; }

        private bool _hasStarted;
        bool ITask<NPCController>.HasStarted
        {
            get => _hasStarted;
            set => _hasStarted = value;
        }

        void ITask<NPCController>.Start(NPCController context)
        {
            context.barco = Barco.Instance;
            IsComplete = true;
        }

        void ITask<NPCController>.Execute(NPCController context)
        {
        }

        public ITask<NPCController> Clone() => new TaskAttackShip();
    }
}