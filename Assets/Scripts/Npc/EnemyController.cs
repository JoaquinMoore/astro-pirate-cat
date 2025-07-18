using Unity.Behavior;
using UnityEngine;
using UnityEngine.Pool;
using WeaponSystem;

namespace Npc
{
    // TODO: Borrar esta clase y remplazarla por NPCFacade
    public class EnemyController : MonoBehaviour
    {
        public SpawnWavePool pool;
        public EnemyTags etag;
        public IObjectPool<EnemyController> _NPCtestPool { set => nPCtestPool = value; }
        
        protected IObjectPool<EnemyController> nPCtestPool;

        private BehaviorGraphAgent _agent;
        private WeaponControler _weaponController;
        private Transform _target;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _agent = GetComponent<BehaviorGraphAgent>();
            _weaponController = GetComponent<WeaponControler>();
            _agent.BlackboardReference.GetVariable<GameObject>("target", out var variable);
            _target = variable.Value.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var mustFlip = transform.position.x < _target.position.x;
            _spriteRenderer.flipX = mustFlip;
            _weaponController.MouseAim(_target.position, mustFlip);
        }

        public void GiveRef(SpawnWavePool a, EnemyTags tag)
        {
            pool = a;
            etag = tag;
        }

        public void Death()
        {
            if (pool)
            {
                //pool.EnemyDeathCallBack(this, etag);
            }

            nPCtestPool.Release(this);
        }
    }
}