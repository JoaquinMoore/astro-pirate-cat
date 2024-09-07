using Zenject;
using UnityEngine;
using Assets.Modules.NPCs.Golden.States;
using Assets.Modules.NPCs.Golden.Interfaces;

namespace Assets.Modules.NPCs.Golden
{
    public class Context : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        private State _currentState;
        [Inject]
        public IInput Input { get; private set; }

        private void Start() => _currentState = new Idle(this);
        public void Update() => _currentState.Update();
        public void Move(Vector2 direction) => transform.Translate(direction.normalized * _speed * Time.deltaTime);
        public void SwitchState(State state)
        {
            _currentState.OnExit();
            _currentState = state;
            _currentState.OnEnter();
        }
    }
}
