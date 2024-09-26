using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NPC.Boss.Ship
{
    public class ShipEnemy : BossBase
    {

        private ShipMovement _mov;
        [SerializeField] private Transform _target;

        [SerializeField] private ShipMovement.MoveSettings _movsettings;
        [SerializeField] private SpawnShip.SpawnSettings _spawnsettings = new();
        [SerializeField] private AttackShip.WepSettings _attacksettings = new();




        public SpawnShip.SpawnSettings Spawnsettings => _spawnsettings;
        public AttackShip.WepSettings Attacksettings => _attacksettings;
        public ShipMovement Mov => _mov;
        Vector3 _pos;

        // Start is called before the first frame update
        public override void Start()
        {
            _movsettings.Target = _target;
            _idleSettings.Target = _target;



            foreach (var item in _spawnsettings.SpawnPoints)
            {
                item.Anims = item.GameObjRef.GetComponentInChildren<Animator>();

                var hold = item.GameObjRef.GetComponentsInChildren<Transform>();

                foreach (var trans in hold)
                {
                    if (trans.tag == "SpawnPoint")
                        item.SpawnPointRef = trans;
                }

            }




            _fsm = new();
            _idleSettings.Attacks.Add(new SpawnShip(null, _fsm));
            _idleSettings.Attacks.Add(new AttackShip(null, _fsm));

            _fsm.ChangeState<BossBase>(typeof(IdleBoss), this);

            if (_movsettings.Target != null)
                _pos = _movsettings.Target.position;
            else
                _pos = transform.position;

            _mov = new(_movsettings, this, _idleSettings.AgroDistance);

        }

        // Update is called once per frame
        void Update()
        {
            _fsm.CurrentState.LogicUpdate();
            _slider.value = (float)_movsettings.Hp.PublicCurrentHealth / _movsettings.Hp.PublicMaxHealth;

        }
        public override void SetTarget(Transform target)
        {
            _target = target;
            _mov.SetTarget(target);
            _pos = target.position;

        }
        public override Transform GetTarget()
        {
            return _target;
        }


        public override void OnDeath()
        {
            
            _anim.SetBool("Death", true);
        }

        public override void OnDamaged()
        {
            _anim.SetTrigger("Hit");
        }

        private void FixedUpdate()
        {
            _mov.Movement();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_pos, _movsettings.MaxTargetDistane);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pos, _movsettings.MinTargetDistane);

        }

    }
}