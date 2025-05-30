using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.FSM;

namespace NPC.Boss
{
    public class IdleBoss : State<BossBase>
    {
        protected IdleSettings _settings;
        protected int AttackId;
        protected bool Stop;
        protected Vector3 _startingPos;

        protected Transform _target;

        public IdleBoss(BossBase boss, FiniteStateMachine fsm) : base(boss, fsm)
        {
            _settings = boss.IdleSettings;

            if (_settings.Target == null)
            {
                _startingPos = boss.transform.position;
            }
            else
            {
                _target = _settings.Target;
                _startingPos = _target.transform.position;
            }


        }


        public override void Enter()
        {
            Stop = false;

            if (context._stop != true)
                context.Anim.SetBool("IsMove",true);
        }

        public override void LogicUpdate()
        {
            if (_target != context.GetTarget())
            {
                Debug.Log("retarget");
                _target = context.GetTarget();
                _startingPos = _target.transform.position;
            }


            if (GameManager.Instance.CheckPlayerDistance(_startingPos, _settings.AgroDistance) && !Stop)
            {
                Stop = true;
                context.StartCoroutine(Timer());
            }
        }


        public override void Exit()
        {
            context.StopAllCoroutines();
        }


        public IEnumerator Timer()
        {
            yield return new WaitForSeconds(_settings.AttackTimer);


            if (_settings.Secuential_Or_Random)
                RandomAttack();
            else
                SecuantianlAttack();

        }


        public void RandomAttack()
        {
            fsm.ChangeState<BossBase>(_settings.Attacks[Random.Range(0, _settings.Attacks.Count)].GetType(), context);
        }

        public void SecuantianlAttack()
        {
            AttackId++;
            AttackId = (int)Mathf.Repeat(AttackId, _settings.Attacks.Count);
            fsm.ChangeState<BossBase>(_settings.Attacks[AttackId].GetType(), context);
        }


        [System.Serializable]
        public class IdleSettings : StateSettings
        {
            [HideInInspector] public Transform Target;
            public bool Secuential_Or_Random;
            public float AttackTimer;
            public float AgroDistance;

            public List<State> Attacks = new();
        }
    }
}