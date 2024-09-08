using Zenject;
using UnityEngine;
using Assets.Modules.NPCs.Golden.States;
using Assets.Modules.NPCs.Golden.Interfaces;
using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden
{
    public class Context : StateContext<Context>
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
