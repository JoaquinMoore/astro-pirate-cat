using System.Collections;
using System.Collections.Generic;
using Npc;
using UnityEngine;
using UnityEngine.Pool;
using WeaponSystem;
using UnityEngine.Events;
using TaskSystem.TaskWrappers;


public class WaveManager : MonoBehaviour//, IPause
{


    [Header("Main Config")]
    [SerializeField] List<WaveBase> _wave = new();
    [SerializeField, Tooltip("Sirve para indicar a que oleada el juego esta listo para terminar (nota: esto termina con el ciclo de oleadas y habra que reiniciarlo una ves este la opcion añadida)")]
    private int _finalWave;
    [SerializeField, Tooltip("Cuanto tiempo queres que el indicador de la oleada quede en pantalla")]
    private float _animWaitTimer;




    [Header("Wave Config")]
    [SerializeField, Tooltip("el intervalo entre cada spawn de enemigos")]
    private float _spawnInterval;
    [SerializeField, Tooltip("La distancia entre los puntos de spawns y el jugador minima que se requiere para spawnear un enemigo")]
    private float _playerDistance;
    [SerializeField, Tooltip("Cada cuantas oleadas queres que se cree la oleada especial")]
    private int _specialWave;

    [Header("Wave Spawn Config")]
    [SerializeField] private float _maxAreaOfSpawn;
    [SerializeField] private float _minAreaOfSpawn;

    [Header("Wave Spawn debug")]
    private float _limitsize;


    [Header("Wave debug")]
    private List<EnemyList> _enemyLists = new();
    private float _totalEnemies;
    private List<NPCController> _enemy = new();
    private float _nextSpecialWave;


    [Header("debug wave timer")]
    private int _indexWave = 0;
    private WaveBase _currentWave;
    private bool _freemode;
    private float _visualTimer;
    public static int IndexWave;

    public static UnityEvent Sendspawns = new();
    public static UnityEvent<List<TypeAmount>, List<Transform>> ReciveSpawns = new();

    private bool _wavePlaying;

    #region Unity Functions
    void Start()
    {
        GameManager._wave = this;
        _limitsize = GameManager.Instance._limitsize;
        _currentWave = _wave[Random.Range(0, _wave.Count)];
        IndexWave = _indexWave;

        ReciveSpawns.AddListener(StartSpawning);
        _nextSpecialWave += _specialWave;
        var holder = EnemyManager.Instance.GetEnemyList();
        foreach (var item in holder)
        {
            EnemyList enemyhold = new();
            enemyhold.Tag = item;
            _enemyLists.Add(enemyhold);
        }


        StartCoroutine(WaveWaitTimer());
    }

    private void LateUpdate()
    {
        if (_wavePlaying)
            CheckEnemys();
    }

    #endregion

    #region Wave Timer Managment
    private void StartWave()
    {
        _indexWave++;
        IndexWave = _indexWave;
        StartSpecial();
        UIManager.MenuManager.SwichMenuState(2, true);
        StartCoroutine(AnimTimer());
        StartSpawning(null ,null);
        _currentWave.Event();
    }

    private void WaveFinished()
    {
        if (_currentWave.WaveChilds.Count == 0)
            return;

        if (_indexWave >= _finalWave && _freemode == false)
        {
            GameManager.Instance.WinState();
            return;
        }

        _visualTimer = 0;
        _wave.Clear();
        _wave = new List<WaveBase>(_currentWave.WaveChilds);
        _currentWave = _wave[Random.Range(0, _wave.Count)];
        StartCoroutine(WaveWaitTimer());
    }


    private IEnumerator AnimTimer()
    {
        yield return new WaitForSeconds(_animWaitTimer);
        UIManager.MenuManager.SwichMenuState(2,false);
    }

    private IEnumerator WaveWaitTimer()
    {
        yield return new WaitForSeconds(_currentWave.TimeForWave);
        StartWave();
    }

    public float WaveProgress()
    {
        float progress = 0;
        if (_wavePlaying)
        {
            progress = _enemy.Count / _totalEnemies;
        }
        else
        {
            _visualTimer += Time.deltaTime;
            progress = Mathf.Clamp(_visualTimer / _currentWave.TimeForWave, 0, 1f);
        }
        return progress;
    }

    #endregion


    #region Wave Stuff

    private void StartSpecial()
    {
        if (IndexWave == _nextSpecialWave)
        {
            _nextSpecialWave += _specialWave;
            Sendspawns?.Invoke();
        }
    }


    private void CheckEnemys()
    {
        if (_enemy.Count == 0)
        {
            StopAllCoroutines();
            WaveFinished();
            _totalEnemies = 0;
            foreach (var item in _enemyLists)
            {
                item.Enemys.Clear();
                item.MaxAmount = 0;
            }
            _wavePlaying = false;
        }
    }


    private void StartSpawning(List<TypeAmount> types, List<Transform> spawnpoint)
    {
        var wave = _currentWave as StandarWave;
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
            StartCoroutine(SpawnEnemies(EnemyList, null, wave.AmauntPerWave));
        else
            StartCoroutine(SpawnEnemies(EnemyList, spawnpoint, types));

    }


    private IEnumerator SpawnEnemies(List<EnemyList> enemylist, List<Transform> spawnpoints, List<TypeAmount> wavespawns)
    {

        int index = wavespawns.Count;
        Vector2 spawnPoint;
        NPCController enemy = null;
        int totalEnemies = 0;
        int currentEnemies = 0;

        int failsafe = 0;



        foreach (var item in wavespawns)
        {
            var holder = enemylist.Find(enemy => enemy.Tag == item.tag);
            if (holder != null)
                holder.MaxAmount = item.AmountForWave;
        }

        foreach (var item in wavespawns)
            totalEnemies += item.AmountForWave;

        for (currentEnemies = 0; currentEnemies < totalEnemies;)
        {
            var holder = wavespawns[Random.Range(0, index)];
            EnemyList listholder = enemylist.Find(enemy => enemy.Tag == holder.tag);

            if (spawnpoints != null && spawnpoints.Count != 0)
               spawnPoint = spawnpoints[Random.Range(0, spawnpoints.Count)].position;
            else
               spawnPoint = SpawnRandomPoint();


            if (failsafe > 250)
            {
                Debug.LogError("overflow of spawner");
                break;
            }
            if (CheckForDistance(spawnPoint, GameManager.Instance.player.transform.position, _playerDistance)
                || CheckForDistance(spawnPoint, transform.position, _minAreaOfSpawn))
            {
                failsafe++;
                continue;
            }

            if (listholder.Enemys.Count < listholder.MaxAmount)
            {
                enemy = EnemyManager.Instance.RequestEnemy(listholder.Tag);
                enemy.transform.position = spawnPoint;
                enemy.Target = Ship.Instance.gameObject;
                //enemy.SetUp();
                //enemy.SetDefaultTask(_Attacktask.Clone());
                currentEnemies++;
                listholder.Enemys.Add(enemy);
                enemy.GiveRef(this);
                enemy.SpanwReset();
                _enemy.Add(enemy);
                failsafe--;
                yield return new WaitForSeconds(_spawnInterval);
            }
            failsafe++;
        }
        _totalEnemies += currentEnemies;
        _wavePlaying = true;
    }


    private bool CheckForDistance(Vector2 point, Vector2 target, float distance)
    {
        return Vector3.Distance(point, target) < distance;
    }


    private Vector2 SpawnRandomPoint()
    {
        float holde = (Random.Range(_minAreaOfSpawn + 2, _maxAreaOfSpawn) + _limitsize);
        Vector2 point = Random.insideUnitCircle.normalized * holde;
        return point;
    }




    public void EnemyDeathCallBack(NPCController npc)
    {
        EnemyList listholder = _enemyLists.Find(x => x.Tag == npc);
        _enemy.Remove(npc);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _maxAreaOfSpawn + _limitsize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _minAreaOfSpawn + _limitsize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _limitsize);
    }

    [System.Serializable]
    public class EnemyList
    {
        public NPCController Tag;
        public int MaxAmount;
        public List<NPCController> Enemys = new();
    }

    [System.Serializable]
    public struct TypeAmount
    {
        public NPCController tag;
        public int AmountForWave;
    }

    #endregion


}

