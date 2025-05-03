using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using WeaponSystem;

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
    [SerializeField, Tooltip("El area donde los enemigos pueden spawnear alrededor de un punto de spawns")]
    private float _AreaOfSpawn;
    [SerializeField, Tooltip("el tiempo que uno decide para el failsafe con cada enemigos, este valor se multiplica con la cantidad total de enemigos")]
    private float _TimeFromEachSpawn;
    [SerializeField, Tooltip("este es un tiempo extra que se a�ade al failsafe encima del tiempo en cada enemigo")]
    private float _ExtraTime;
    [SerializeField, Tooltip("Cada cuantas oleadas queres que se cree la oleada especial")]
    private int _SpecialWave;



    [SerializeField, Tooltip("Pools de los enemigos")]
    private List<EnemyPool> _enemyPools;
    [SerializeField, Tooltip("Puntos de spawn para los enemigos")]
    List<SpawnPoints> _SpawnPoints = new();


    [Header("debug")]
    [SerializeField] private float _currentEnemies = 0;
    [SerializeField] private float _totalEnemies;
    [SerializeField] private List<EnemyController> _enemy;
    [SerializeField] private List<EnemyList> _enemyLists = new();
    [SerializeField] private List<TypeAmount> _waveSpawns = new();
    [SerializeField] private float _NextSpecialWave;

    public static UnityEvent<List<TypeAmount>, List<SpawnPoints>> ReciveSpawns = new();

    public static UnityEvent Sendspawns = new();

    private bool TimerStarted;

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
            TempPool<EnemyController> buildtest = new(item.Create);
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




    public void OnGetFromPool(EnemyController pooledBullet)
    {
        pooledBullet.gameObject.SetActive(true);
    }

    public void OnReleaseToPool(EnemyController pooledBullet)
    {
        pooledBullet.gameObject.SetActive(false);
    }

    public void OnDestroyPooledObject(EnemyController pooledBullet)
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
        EnemyController enemy = null;
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
            var holder = waveSpawns[Random.Range(0, index)];
            EnemyList listholder = EnemyList.Find(enemy => enemy.Tag == holder.tag);
            EnemyPool poolholder = _enemyPools.Find(enemy => enemy.tag == holder.tag);

            spawnPoint = CheckForPlayerView(spawnpoint[Random.Range(0, spawnpoint.Count)]);



            if (spawnPoint.InView)
                continue;

            if (listholder.Enemys.Count < listholder.MaxAmount)
            {
                enemy = poolholder.Pool.Get();
                enemy.transform.position = (Vector2)spawnPoint.SpawnPoint.position + (Random.insideUnitCircle * _AreaOfSpawn);
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


    public void EnemyDeathCallBack(EnemyController npc, EnemyTags tag)
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
        yield return new WaitForSeconds((_TimeFromEachSpawn * _totalEnemies) + _ExtraTime);

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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(item.SpawnPoint.position, _PlayerDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(item.SpawnPoint.position, _AreaOfSpawn);
        }
    }

    [System.Serializable]
    public class EnemyList
    {
        public EnemyTags Tag;
        public int MaxAmount;
        public List<EnemyController> Enemys = new();
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
        public EnemyController EnemyPrefab;
        public IObjectPool<EnemyController> Pool;
        [HideInInspector] public SpawnWavePool Father;

        public EnemyController Create()
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
