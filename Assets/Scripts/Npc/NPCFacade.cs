using Physics.Movement;
using UnityEngine;
using UnityServiceLocator;

namespace Npc
{
    public class NPCFacade : MonoBehaviour, IInteractor
    {
        private Enumerators.Team _team;
        private MovementService _movement;

        private void Start()
        {
            ServiceLocator.For(this)
                .Get(out _movement);
        }

        public void MoveTo(Vector2 destiny)
        {
            _movement.GoTo(destiny);
        }
    }
}