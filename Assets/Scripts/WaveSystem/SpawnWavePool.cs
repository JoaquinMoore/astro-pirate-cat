using System.Collections;
using System.Collections.Generic;
using Npc;
using UnityEngine;
using UnityEngine.Pool;
using WeaponSystem;
using UnityEngine.Events;
using TaskSystem.TaskWrappers;
public enum EnemyTags
{
    Golden,
    Chiguagua,
    Bulldog,
    Nave,
    Caniche,
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
    [SerializeField] private TaskWrapperSO _Attacktask;


    [SerializeField, Tooltip("Pools de los enemigos")]
    private List<EnemyPool> _enemyPools;
    [SerializeField, Tooltip("Puntos de spawn para los enemigos")]
    List<SpawnPoints> _SpawnPoints = new();


    [Header("debug")]
    [SerializeField] private float _currentEnemies = 0;
    [SerializeField] private float _totalEnemies;
    [SerializeField] private List<EnemyController> _enemy;
    [SerializeField] private List<EnemyList> _enemyLists = new();
    [SerializeField] private float _NextSpecialWave;
    private WaveManager _manager;

    public static UnityEvent<List<TypeAmount>, List<SpawnPoints>> ReciveSpawns = new();

    public static UnityEvent Sendspawns = new();

    private bool _timerStarted;
    private bool _wavePlaying;

    void Start()
    {
        _NextSpecialWave += _SpecialWave;
        ReciveSpawns.AddListener(StartSpawning);
        _manager = GetComponentInParent<WaveManager>();
        foreach (var item in _SpawnPoints)
        {
            item.SpawnPoint.LookAt(transform.position);
        }




        foreach (var item in _enemyPools)
        {
            TempPool<NPCController> buildtest = new(item.Create);
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




    public void OnGetFromPool(NPCController pooledBullet)
    {
        pooledBullet.gameObject.SetActive(true);
    }

    public void OnReleaseToPool(NPCController pooledBullet)
    {
        pooledBullet.gameObject.SetActive(false);
    }

    public void OnDestroyPooledObject(NPCController pooledBullet)
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
        if (_wavePlaying)
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
        Debug.Log("checking");
        if (_currentEnemies <= 0)
        {
            StopAllCoroutines();
            //_manager.WaveFinished();
            WaveManager.FinishedWave?.Invoke();
            _timerStarted = false;
            _totalEnemies = 0;
            foreach (var item in _enemyLists)
            {
                item.Enemys.Clear();
                item.MaxAmount = 0;
            }
            _wavePlaying = false;
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
        int index = waveSpawns.Count;
        SpawnPoints spawnPoint;
        NPCController enemy = null;
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
                enemy.transform.position = (Vector2)spawnPoint.SpawnPoint.position + (Random.insideUnitCircle *_AreaOfSpawn);
                enemy.SetDefaultTask(_Attacktask.Clone());
                currentEnemies++;
                listholder.Enemys.Add(enemy);
                failsafe--;
                yield return new WaitForSeconds(_spawnInterval);
            }
            failsafe++;
            if (failsafe > 250)
            {
                Debug.LogError("overflow of spawner");
                break;
            }
        }
        _totalEnemies += currentEnemies;
        _currentEnemies += currentEnemies;

        if (_timerStarted == false)
            StartCoroutine(TimerFailSafe());
        //_wavePlaying = false;
        foreach (var item in EnemyList)
        {
            var hold = _enemyLists.Find(x => x.Tag == item.Tag);
            hold.MaxAmount += item.MaxAmount;
            foreach (var itema in item.Enemys)
            {
                hold.Enemys.Add(itema);
            }

        }
        _wavePlaying = true;
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


    public void EnemyDeathCallBack(NPCController npc, EnemyTags tag)
    {
        EnemyList listholder = _enemyLists.Find(x => x.Tag == tag);
        Debug.Log("Death");
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
        _timerStarted = true;
        yield return new WaitForSeconds((_TimeFromEachSpawn * _totalEnemies)+ _ExtraTime );

        foreach (var item in _enemyLists)
        {
            item.Enemys.Clear();
            item.MaxAmount = 0;
        }
        _currentEnemies = 0;
        _totalEnemies = 0;
        _timerStarted = false;
        WaveManager.FinishedWave?.Invoke();
        _wavePlaying = false;
        //_manager.WaveFinished();
    }


    public void PushSpawners(float newdistance)
    {
        foreach (var item in _SpawnPoints)
        {
            item.SpawnPoint.transform.position -= item.SpawnPoint.transform.forward * newdistance;
        }
    }



    public float WaveProgress()
    {
        float progress = 1;
        if (_timerStarted)
        {
            progress = (float)_currentEnemies / _totalEnemies;
        }
        Debug.Log(progress);
        return progress;
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
        public List<NPCController> Enemys = new();
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
        public NPCController EnemyPrefab;
        public IObjectPool<NPCController> Pool;
        [HideInInspector] public SpawnWavePool Father;

        public NPCController Create()
        {
            var holder = Instantiate(EnemyPrefab);
            holder.GiveRef(Father, tag);
            holder._NPCPool = Pool;
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
