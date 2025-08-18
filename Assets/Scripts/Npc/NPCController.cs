using System;
using Physics.Movement;
using TaskSystem;
using Unity.Behavior;
using UnityEngine;
using UnityServiceLocator;
using WeaponSystem;
using UnityEngine.Pool;

namespace Npc
{
    public class NPCController : MonoBehaviour
    {
        
        public Data NPCData => _data;

        public GameObject Target
        {
            get {
                Agent.BlackboardReference.GetVariableValue("Enemy", out GameObject go);
                return go;
            }
            set => Agent.BlackboardReference.SetVariableValue("Enemy", value);
        }

        public bool Stunned
        {
            get
            {
                Agent.BlackboardReference.GetVariableValue("Stunned", out bool go);
                return go;
            }
            set => Agent.BlackboardReference.SetVariableValue("Stunned", value);
        }

        private SpriteRenderer _sprite;


        private BehaviorGraphAgent _agent;
        private Data _data;
        private Enumerators.Team _team;
        private MovementService _movement;
        private TasksController<NPCController> _tasksController;
        private WeaponControler _weaponController;

        private MovementService Movement => _movement ??= ServiceLocator.For(gameObject).Get<MovementService>();
        private TasksController<NPCController> TasksController => _tasksController ??= ServiceLocator.For(gameObject).Get<TasksController<NPCController>>();
        private BehaviorGraphAgent Agent => _agent ??= ServiceLocator.For(gameObject).Get<BehaviorGraphAgent>();



        private SpawnWavePool _pool;
        private EnemyPoolData _etag;
        public IObjectPool<NPCController> _NPCPool { set => NPCPool = value; }

        protected IObjectPool<NPCController> NPCPool;

        [Header("test")]
        public bool ElBoolQueTeMata;
        private void Start()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            ServiceLocator.For(gameObject).TryGet(out _data);
            _movement = Movement;
            _tasksController = TasksController;
            ServiceLocator.For(gameObject).TryGet(out _weaponController);
            Agent.BlackboardReference.SetVariableValue("AttackDistance", _data.AttackDistance);
            Agent.BlackboardReference.SetVariableValue("Stunned Time", _data.StunnedTime);
        }

        public void SpanwReset()
        {
            var hold = GetComponentInChildren<HealthSystem.Health>();
            Stunned = false;
            Agent.BlackboardReference.SetVariableValue("Stunned", false);
            hold.Heal(hold.PublicMaxHealth);
        }


        private void Update()
        {
            _movement.VirtualUpdate();
            

            //_weaponcontroller.MouseAim(PWorldPosition(Mouse.current), mustFlip);
            if (Target)
            {
                var mustFlip = Target.transform.position.x > transform.position.x;
                _weaponController.MouseAim(Target.transform.position, mustFlip);
                HorizontalFlip(mustFlip);
            }
            else
            {

                _weaponController.MouseAim(Vector2.right* (_movement.CurrentDestiny.x > transform.position.x == true ? -2 : 2), _movement.CurrentDestiny.x > transform.position.x);
                HorizontalFlip(_movement.CurrentDestiny.x > transform.position.x);
            }
            if (ElBoolQueTeMata)
            {
                Death();
                ElBoolQueTeMata = false;
            }

        }

        public void SetDefaultTask(ITask<NPCController> baseTask) => TasksController.DefaultTask = baseTask;

        public void CheckTask() => TasksController.CheckTask();

        public void MoveTo(Vector2 position)
        {
            _movement.GoTo(position);
        }

        public void TriggerDown() => _weaponController.PrimaryFireDown();
        public void TriggerUp() => _weaponController.PrimaryFireUp();


        public void OnHooked()
        {
            Debug.Log("stunned");
            Stunned = true;
        }

        public void HorizontalFlip(bool flag)
        {
            //transform.localScale = new Vector3(flag ? -1 : 1, transform.localScale.y, transform.localScale.z);
            _sprite.flipX = flag;

        }

        public void GiveRef(SpawnWavePool a, EnemyPoolData tag)
        {
            _pool = a;
            _etag = tag;
        }

        public void Death()
        {
            if (_pool)
                _pool.EnemyDeathCallBack(this, _etag);
            if (NPCPool != null)
                NPCPool.Release(this);
            else
                Destroy(gameObject);
            Stunned = false;
        }


        [Serializable]
        public class Data
        {
            public SteeringMovementDataSO _movementData;
            public float ViewDistance;
            public float AttackDistance;
            public float StunnedTime;
            public string[] DetectionTags;
        }
    }
}