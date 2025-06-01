using System;
using System.Collections;
using _EXTENSIONS;
using Npc.Tasks.Interfaces;
using UnityEngine;

namespace Npc.Tasks
{
    [Serializable]
    public class TaskInteract : ITask
    {
        private const float INTERACTION_THRESHOLD = 1f;

        private IInteractable _target;

        public TaskInteract(IInteractable target)
        {
            _target = target;
        }

        public void Execute(NPCFacade npc)
        {
            Debug.Log(npc);
            npc.StartCoroutine(GoToTask(npc));
        }

        private IEnumerator GoToTask(NPCFacade npc)
        {
            Vector2 interactionSpot;

            do
            {
                interactionSpot = _target.GetInteractionPosition(npc.transform.position);
                npc.GoTo(interactionSpot);
                yield return null;
            } while (!npc.transform.position.AreApproximately(interactionSpot, INTERACTION_THRESHOLD));

            _target.Interact(npc);
        }
    }
}