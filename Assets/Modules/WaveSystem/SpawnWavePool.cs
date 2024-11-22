using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using WeaponSystem;
using UnityEngine.Events;

public enum EnemyTags
{
    test1,
    test2,
    test3
}


public class SpawnWavePool : MonoBehaviour
{
    [Header("Config")]
    [SerializeField, Tooltip("el intervalo entre cada spawn de enemigos")]
    private float _spawnInterval;
    [SerializeField, Tooltip("La distancia entre los puntos de spawns y el jugador minima que se requiere para spawnear un enemigo")]
    private float _PlayerDistance;
    [SerializeField, Tooltip("el tiempo que uno decide para el failsafe con cada enemigos, este valor se multiplica con la cantidad total de enemigos")]
    private float _TimeFromEachSpawn;
    [SerializeField, Tooltip("este es un tiempo extra que se añade al failsafe encima del tiempo en cada enemigo")]
    private float _ExtraTime;
    [SerializeField, Tooltip("Cada cuantas oleadas queres que se cree la oleada especial")]
    private float _SpecialWave;

    [SerializeField, Tooltip("Pools de los enemigos")]
    private List<EnemyPool> _enemyPools;
    [SerializeField, Tooltip("Puntos de spawn para los enemigos")]
    List<SpawnPoints> _SpawnPoints = new();


    [Header("debug")]
    [SerializeField] private float _currentEnemies = 0;
    [SerializeField] private float _totalEnemies;
    [SerializeField] private List<NPCtestPool> _enemy;
    [SerializeField] private List<EnemyList> _enemyLists = new();
    [SerializeField] private List<TypeAmount> _waveSpawns = new();
    [SerializeField] private float _NextSpecialWave;

    public static UnityEvent<List<TypeAmount>, List<SpawnPoints>> ReciveSpawns = new();

    public static UnityEvent Sendspawns = new();

    private bool TimerStarted;

    //private IObjectPool<testenemy> pool;

    void Start()
    {
        _NextSpecialWave += _SpecialWave;
        ReciveSpawns.AddListener(StartSpawning);
        foreach (var item in _SpawnPoints)
        {
            item.SpawnPoint.LookAt(transform.position);
        }




        foreach (var item in _enemyPools)
        {
            TempPool<NPCtestPool> buildtest = new(item.Create);
            buildtest.SetActionOnGet(OnGetFromPool);
            buildtest.SetActionOnRelease(OnReleaseToPool);
            buildtest.SetActionOnDestroy(OnDestroyPooledObject);
            buildtest.SetDefaultCapacity(100);
            buildtest.SetMaxSize(200);
            item.Pool = buildtest.Build();


            item.Father = this;
            EnemyList hold = new()
            {
                Tag = item.tag,
            };
            _enemyLists.Add(hold);
        }

        //_enemyPools[0].Create();
    }




    public void OnGetFromPool(NPCtestPool pooledBullet)
    {
        pooledBullet.gameObject.SetActive(true);
    }

    public void OnReleaseToPool(NPCtestPool pooledBullet)
    {
        pooledBullet.gameObject.SetActive(false);
    }

    public void OnDestroyPooledObject(NPCtestPool pooledBullet)
    {
        Destroy(pooledBullet.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PushSpawners(5);
        }
    }


    private void LateUpdate()
    {
        if (_totalEnemies > 0)
            CheckEnemys();

    }
    public void StartSpecial()
    {
        if (WaveManager.IndexWave == _NextSpecialWave)
        {
            _NextSpecialWave += _SpecialWave;
            Sendspawns?.Invoke();
        }
    }


    public void CheckEnemys()
    {
        if (_currentEnemies <= 0)
        {
            StopAllCoroutines();
            WaveManager.FinishedWave?.Invoke();
            Debug.Log("end");
            _totalEnemies = 0;
            foreach (var item in _enemyLists)
            {
                item.Enemys.Clear();
                item.MaxAmount = 0;
            }
        }
    }


    public void StartSpawning(List<TypeAmount> waveSpawns, List<SpawnPoints> spawnpoint = null)
    {
        int stop = 0;

        foreach (var item in _SpawnPoints)
        {
            CheckForPlayerView(item);
            if (item.InView)
                stop++;
        }

        if (stop == _SpawnPoints.Count)
            return;
        List<EnemyList> EnemyList = new();





        foreach (var item in _enemyLists)
        {
            EnemyList hold = new()
            {
                Tag = item.Tag,
            };
            EnemyList.Add(hold);
        }

        if (spawnpoint == null || spawnpoint.Count == 0)
            StartCoroutine(SpawnEnemies(EnemyList, _SpawnPoints, waveSpawns));
        else
            StartCoroutine(SpawnEnemies(EnemyList, spawnpoint, waveSpawns));

    }


    IEnumerator SpawnEnemies(List<EnemyList> EnemyList, List<SpawnPoints> spawnpoint, List<TypeAmount> waveSpawns)
    {
        Debug.Log("inicio");
        int index = waveSpawns.Count;
        SpawnPoints spawnPoint;
        NPCtestPool enemy = null;
        int totalEnemies = 0;
        int currentEnemies = 0;

        int failsafe = 0;

        foreach (var item in EnemyList)
        {
            foreach (var enemys in waveSpawns)
            {
                if (item.Tag == enemys.tag)
                    item.MaxAmount = enemys.AmountForWave;
            }
        }
    
        foreach (var item in waveSpawns)
            totalEnemies += item.AmountForWave;
    
        for (currentEnemies = 0; currentEnemies < totalEnemies;)
        {
            var holder = waveSpawns[UnityEngine.Random.Range(0, index)];
            EnemyList listholder = EnemyList.Find(enemy => enemy.Tag == holder.tag);
            EnemyPool poolholder = _enemyPools.Find(enemy => enemy.tag == holder.tag);

            spawnPoint = CheckForPlayerView(spawnpoint[UnityEngine.Random.Range(0, spawnpoint.Count)]);



            if (spawnPoint.InView)
                continue;

            if (listholder.Enemys.Count < listholder.MaxAmount)
            {
                enemy = poolholder.Pool.Get();
                enemy.transform.position = spawnPoint.SpawnPoint.position;
                currentEnemies++;
                listholder.Enemys.Add(enemy);
                failsafe--;
                yield return new WaitForSeconds(_spawnInterval);
            }
            failsafe++;
            if (failsafe > 150)
            {
                Debug.LogError("overflow of spawner");
                break;
            }
        }
        _totalEnemies += totalEnemies;
        _currentEnemies += currentEnemies;

        if (TimerStarted == false)
            StartCoroutine(TimerFailSafe());

        foreach (var item in EnemyList)
        {
            var hold = _enemyLists.Find(x => x.Tag == item.Tag);
            hold.MaxAmount += item.MaxAmount;
            foreach (var itema in item.Enemys)
            {
                hold.Enemys.Add(itema);
            }

        }

        //if (_AllBasesAttackWave == WaveManager.IndexWave)
        //{
        //    StartSpecialWave();
        //}
    }


    public SpawnPoints CheckForPlayerView(SpawnPoints point)
    {
        point.InView = Vector3.Distance(point.SpawnPoint.position, GameManager.Instance.player.transform.position) < _PlayerDistance;
        return point;
    }


    public void EnemyDeathCallBack(NPCtestPool npc, EnemyTags tag)
    {
        EnemyList listholder = _enemyLists.Find(x => x.Tag == tag);

        if (listholder == null)
            return;
        if (listholder.Enemys.Find(x => x == npc))
        {
            listholder.Enemys.Remove(npc);
            _currentEnemies--;
        }



    }


    IEnumerator TimerFailSafe()
    {
        TimerStarted = true;
        yield return new WaitForSeconds((_TimeFromEachSpawn * _totalEnemies)+ _ExtraTime );

        foreach (var item in _enemyLists)
        {
            item.Enemys.Clear();
            item.MaxAmount = 0;
        }
        _currentEnemies = 0;
        _totalEnemies = 0;
        TimerStarted = false;
        WaveManager.FinishedWave?.Invoke();
    }


    public void PushSpawners(float newdistance)
    {
        foreach (var item in _SpawnPoints)
        {
            item.SpawnPoint.transform.position -= item.SpawnPoint.transform.forward * newdistance;
        }
    }


    private void OnDrawGizmos()
    {
        foreach (var item in _SpawnPoints)
        {
            Gizmos.DrawWireSphere(item.SpawnPoint.position, _PlayerDistance);
        }
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
        public IObjectPool<NPCtestPool> Pool;
        [HideInInspector] public SpawnWavePool Father;

        public NPCtestPool Create()
        {
            var holder = Instantiate(EnemyPrefab);
            holder.GiveRef(Father, tag);
            holder._NPCtestPool = Pool;
            return holder;
        }
    }


    [System.Serializable]
    public class SpawnPoints
    {
        public Transform SpawnPoint;
        public bool InView;
    }
}
