using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;
using NPC.FSM;

namespace NPC.Boss
{
    public class BossBase : MonoBehaviour
    {
        [SerializeField] protected Animator _anim;
        [SerializeField] protected Health _hp;

        protected FiniteStateMachine _fsm;

        [SerializeField] protected IdleBoss.IdleSettings _idleSettings = new();

        public IdleBoss.IdleSettings IdleSettings => _idleSettings;


        // Start is called before the first frame update
        public virtual void Start()
        {
            _fsm = new();
        }

        // Update is called once per frame
        void Update()
        {

        }




        public virtual void OnDamaged() { }
        public virtual void OnDeath() { }

        public virtual void SetTarget(Transform target) { }
        public virtual Transform GetTarget() { return default; }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _idleSettings.AgroDistance);
        }

    }
}