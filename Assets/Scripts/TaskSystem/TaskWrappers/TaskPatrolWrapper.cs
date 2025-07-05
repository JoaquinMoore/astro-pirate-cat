using Npc;
using TaskSystem.Tasks;
using UnityEngine;

namespace TaskSystem.TaskWrappers
{
    [CreateAssetMenu(fileName = "TaskPatrol", menuName = "Tasks/Patrol")]
    public class TaskPatrolWrapper : TaskWrapperSO
    {
        public Vector3[] patrolPoints;

        public override ITask<NPCController> Clone() => new TaskPatrol(patrolPoints);
    }
}