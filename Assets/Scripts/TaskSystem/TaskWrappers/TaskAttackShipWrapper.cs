using Npc;
using TaskSystem;
using TaskSystem.Tasks;
using TaskSystem.TaskWrappers;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskAttackShip", menuName = "Tasks/Attack Ship")]
public class TaskAttackShipWrapper : TaskWrapperSO
{
    public override ITask<NPCController> Clone() => new TaskAttackShip();
}