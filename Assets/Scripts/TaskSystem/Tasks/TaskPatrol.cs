using Npc;
using UnityEngine;

namespace TaskSystem.Tasks
{
    public class TaskPatrol : ITask<NPCController>
    {
        public bool IsComplete { get; }
        bool ITask<NPCController>.HasStarted { get; set; }

        private Vector3[] _patrolPoints;

        public TaskPatrol(Vector3[] patrolPoints)
        {
            _patrolPoints = patrolPoints;
        }

        void ITask<NPCController>.Start(NPCController context)
        {
        }

        void ITask<NPCController>.Execute(NPCController context)
        {
        }

        public ITask<NPCController> Clone() => new TaskPatrol(_patrolPoints);
    }
}