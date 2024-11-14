using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;
using WeaponSystem;
public enum EnemyTags
{
    test1,
    test2,
    test3
}


public class testwavepool : MonoBehaviour
{
    [SerializeField, Tooltip("el intervalo entre cada spawn de enemigos")]
    private float _spawnInterval;
    [SerializeField] List<Transform> _spawnPoints = new();
    [SerializeField] private List<TypeAmount> TestSpawn = new();
    [SerializeField] private List<EnemyList> EnemyLists = new();
    [SerializeField] private float _currentEnemies = 0;
    [SerializeField] private float _totalEnemies;



    [SerializeField] private List<NPCtestPool> enemy;
    [SerializeField] private List<EnemyPool> EnemyPools;
    //private IObjectPool<testenemy> pool;


    void Start()
    {
        foreach (var item in EnemyPools)
        {
            var buildtest = new TempPool<NPCtestPool>(() => Create(item.EnemyPrefab));
            item.pool = buildtest.Build();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    public NPCtestPool Create(NPCtestPool enemy)
    {
        return Instantiate(enemy);
    }


    IEnumerator SpawnEnemies()
    {
        int index = TestSpawn.Count;
        NPCtestPool enemy = null;
    
        foreach (var item in EnemyLists)
        {
            foreach (var enemys in TestSpawn)
            {
                if (item.Tag == enemys.tag)
                    item.MaxAmount = enemys.AmountForWave;
            }
        }
    
        foreach (var item in TestSpawn)
            _totalEnemies += item.AmountForWave;
    
        for (_currentEnemies = 0; _currentEnemies < _totalEnemies;)
        {
            var holder = TestSpawn[UnityEngine.Random.Range(0, index)];
            EnemyList listholder = EnemyLists.Find(enemy => enemy.Tag == holder.tag);
            EnemyPool poolholder = EnemyPools.Find(enemy => enemy.tag == holder.tag);


            if (listholder.Enemys.Count != listholder.MaxAmount)
            {
                enemy = poolholder.pool.Get();
                enemy.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position;
                _currentEnemies++;
                listholder.Enemys.Add(enemy);
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    
    
        //if (_AllBasesAttackWave == WaveManager.IndexWave)
        //{
        //    StartSpecialWave();
        //}
    }


    [System.Serializable]
    public class EnemyList
    {
        public EnemyTags Tag;
        public int MaxAmount;
        public List<NPCtestPool> Enemys = new();
    }

    [System.Serializable]
    public class TypeAmount
    {
        public EnemyTags tag;
        public int AmountForWave;
    }
    [System.Serializable]
    public class EnemyPool
    {
        public EnemyTags tag;
        public NPCtestPool EnemyPrefab;
        public IObjectPool<NPCtestPool> pool;
    }

}
