using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npc;
using UnityEngine.Pool;
using WeaponSystem;
using _UTILITY;
public class EnemyManager : SingletonMono<EnemyManager>
{
    [SerializeField, Tooltip("Pools de los enemigos")]
    private List<EnemyPool> _enemyPools;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    protected override void Awake()
    {
        foreach (var item in _enemyPools)
        {
            PoolAsembly(item);
        }
        base.Awake();
    }

    private void PoolAsembly(EnemyPool enemy)
    {
        WeaponSystem.GenericPool<NPCController> Phold = new(enemy.Create);
        Phold.SetActionOnGet(OnGetFromPool);
        Phold.SetActionOnRelease(OnReleaseToPool);
        Phold.SetActionOnDestroy(OnDestroyPooledObject);
        Phold.SetDefaultCapacity(100);
        Phold.SetMaxSize(1000);
        enemy.Pool = Phold.Build();
    }


    public NPCController RequestEnemy(NPCController tag)
    {
        return _enemyPools.Find(x => x.EnemyPrefab == tag).Create();
    }

    public List<NPCController> GetEnemyList()
    {
        List<NPCController> holder = new();
        foreach (var item in _enemyPools)
            holder.Add(item.EnemyPrefab);
        return holder;
    }

    #region EnemyPool
    public void OnGetFromPool(NPCController pooled)
    {
        pooled.gameObject.SetActive(true);
    }

    public void OnReleaseToPool(NPCController pooled)
    {
        pooled.gameObject.SetActive(false);
    }

    public void OnDestroyPooledObject(NPCController pooled)
    {
        Destroy(pooled.gameObject);
    }

    #endregion


    [System.Serializable]
    public struct EnemyPool
    {
        public NPCController EnemyPrefab;
        public IObjectPool<NPCController> Pool;
        [HideInInspector] public EnemyManager Father;

        public NPCController Create()
        {
            var holder = Instantiate(EnemyPrefab);
            holder._NPCPool = Pool;
            return holder;
        }
    }


}
