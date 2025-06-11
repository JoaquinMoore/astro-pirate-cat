using Physics.Movement;
using TaskSystem;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCBootstrapper : Bootstrapper
    {
        [SerializeField]
        private NPCController.Data _npcData;

        protected override void Bootstrap()
        {
            Register(GetComponent<BehaviorGraphAgent>());
            Register(new MovementService(transform, _npcData._movementData));
            Register(new TasksController<NPCController>(GetComponent<NPCController>()));
            Register(_npcData);
        }
    }
}