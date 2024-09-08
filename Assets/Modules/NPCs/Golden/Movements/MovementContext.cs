using UnityEngine;
using Assets.Modules.Util.StateMachine;
using Assets.Modules.NPCs.Golden.Movements.States;
using Assets.Modules.NPCs.Golden.Movements.Interfaces;

namespace Assets.Modules.NPCs.Golden.Movements
{
    public class MovementContext : StateContext<MovementContext>
    {
        [field: SerializeField] public float Speed { get; private set; }
        public IInput Input { get; private set; }

        private void Start()
        {
            Input = GetComponent<IInput>();
            Initialize(new Idle(this));
        }

        public void Move(Vector2 direction) => transform.Translate(direction.normalized * Speed * Time.deltaTime);
    }
}
