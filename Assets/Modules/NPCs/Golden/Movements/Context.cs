using Zenject;
using UnityEngine;
using Assets.Modules.NPCs.Golden.States;
using Assets.Modules.NPCs.Golden.Interfaces;
using Assets.Modules.Util.StateMachine;

namespace Assets.Modules.NPCs.Golden
{
    public class Context : StateContext<Context>
    {
        [SerializeField]
        private float _speed;
        [Inject]
        public IInput Input { get; private set; }

        private void Start() => Initialize(new Idle(this));
        public void Move(Vector2 direction) => transform.Translate(direction.normalized * _speed * Time.deltaTime);
    }
}
