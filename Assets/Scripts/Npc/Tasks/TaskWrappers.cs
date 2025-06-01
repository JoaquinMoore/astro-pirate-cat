using Npc.Tasks.Interfaces;
using UnityEngine;

namespace Npc.Tasks
{
    public abstract class BaseTaskWrapper
    {
        public abstract ITask GetTask();
    }

    public class TaskInteractWrapper : BaseTaskWrapper
    {
        [SerializeField]
        private GameObject target;

        public override ITask GetTask()
        {
            if (!target.TryGetComponent<IInteractable>(out var interactable))
            {
                Debug.LogError($"El objeto <{target}> no contiene un componente <{typeof(IInteractable)}>");
            }

            return new TaskInteract(Object.FindFirstObjectByType(interactable.GetType()) as IInteractable);
        }
    }

    public class TaskMoveWrapper : BaseTaskWrapper
    {
        [SerializeField]
        private Vector2 position;

        public override ITask GetTask()
        {
            return new TaskMove(position);
        }
    }
}