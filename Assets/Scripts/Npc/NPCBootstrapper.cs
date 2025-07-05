using Physics.Movement;
using TaskSystem;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCBootstrapper : Bootstrapper
    {
        [SerializeField]
        private NPCDataSO _npcData;

        protected override void Bootstrap()
        {
            Register(new MovementService(transform, _npcData.value._movementData));
            Register(new TasksController<NPCController>(GetComponent<NPCController>()));
            Register(_npcData.value);
        }
    }
}