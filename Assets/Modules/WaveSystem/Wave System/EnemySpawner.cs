using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemySpawner : MonoBehaviour//, IPause
{
    [SerializeField] List<Transform> _spawnPoints = new();
    [SerializeField, Tooltip("el intervalo entre cada spawn de enemigos")]
    private float _spawnInterval;
    [SerializeField] private int _AllBasesAttackWave;

    [Header("Visual Wave")]
    [SerializeField, ColorUsage(true, true)] Color _normalColorWave;
    [SerializeField, ColorUsage(true, true)] Color _specialColorWave;

    [SerializeField] private List<EnemyList> EnemyLists = new();

    private List<SpecialWaveData> EnemyBaseData = new();
    private EventWave CurrentWave;
    [SerializeField] private float _currentEnemies = 0;
    [SerializeField] private float _totalEnemies;
    WaveManager _parent;


    //public static UnityEvent<Enemy> Death = new();
    //public static UnityEvent<EventWave> StartWave = new();
    //public static UnityEvent<Enemy, EnemyTags> AddToList = new();



    #region Unity Functions
    void Start()
    {
        //GameManager.Instance?._PauseGame.Add(this);
        //Death.AddListener(DeathEnemy);
        //StartWave.AddListener(ReciveWave);
        var points = GetComponentsInChildren<Transform>();
        if (points.Length - 1 != _spawnPoints.Count)
            for (int i = 1; i < points.Length; i++)
                _spawnPoints.Add(points[i]);

        //foreach (EnemyTags item in Enum.GetValues(typeof(EnemyTags)))
        //{
        //    EnemyList holder = new();
        //    holder.Tag = item;
        //    EnemyLists.Add(holder);
        //}

        _parent = GetComponentInParent<WaveManager>();
    }
    #endregion

    #region Reciving Data
    public void ReciveWave(EventWave wave)
    {
        CurrentWave = wave;

        // Visual Wave normal
        _parent.MatWave.SetColor("_MainColor", _normalColorWave);

        //StartCoroutine(SpawnEnemies());

        if (_AllBasesAttackWave == WaveManager.IndexWave)
        {
            // Visual Wave spacial
            _parent.MatWave.SetColor("_MainColor", _specialColorWave);
        }

    }
    #endregion

    #region Spawn
    //IEnumerator SpawnEnemies()
    //{
    //    int index = CurrentWave.AmauntPerWave.Count;
    //    Enemy enemy = null;
    //
    //    foreach (var item in EnemyLists)
    //    {
    //        foreach (var enemys in CurrentWave.AmauntPerWave)
    //        {
    //            if (item.Tag == enemys.tag)
    //                item.MaxAmount = enemys.AmountForWave;
    //        }
    //    }
    //
    //    foreach (var item in CurrentWave.AmauntPerWave)
    //        _totalEnemies += item.AmountForWave;
    //
    //    for (_currentEnemies = 0; _currentEnemies < _totalEnemies;)
    //    {
    //        var holder = CurrentWave.AmauntPerWave[UnityEngine.Random.Range(0, index)];
    //        EnemyList listholder = FindItem(holder.tag);
    //
    //        if (listholder.Enemys.Count != listholder.MaxAmount)
    //        {
    //            enemy = Instantiate(holder.EnemyPrefab);
    //            enemy.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position;
    //            _currentEnemies++;
    //            listholder.Enemys.Add(enemy);
    //
    //        }
    //        yield return new WaitForSeconds(_spawnInterval);
    //    }
    //
    //
    //    if (_AllBasesAttackWave == WaveManager.IndexWave)
    //    {
    //        StartSpecialWave();
    //    }
    //}

    public void StartSpecialWave()
    {
        //foreach (var item in EnemyBaseControler.AllBases)
        //{
        //    SpecialWaveData holder = new();
        //    holder._BaseSpawnPoint = item.GetWaveTransform();
        //    holder._BaseWaveData = item.GetWaveData();
        //    holder.BossRef = item.GetShip();
        //    EnemyBaseData.Add(holder);
        //}

        foreach (var item in EnemyBaseData)
        {
            //StartCoroutine(SpecialWave(item));
        }
        
    }


    //IEnumerator SpecialWave(SpecialWaveData data)
    //{
    //    int index = data._BaseWaveData.Count;
    //    Enemy enemy = null;
    //    int totalenemies = 0;
    //    int currentenemies = 0;
    //
    //    if (data.BossRef == null)
    //        yield return null;
    //    foreach (var item in EnemyLists)
    //    {
    //        foreach (var enemys in data._BaseWaveData)
    //        {
    //            if (item.Tag == enemys.tag)
    //                item.MaxAmount += enemys.AmountForWave;
    //        }
    //    }
    //
    //    foreach (var item in data._BaseWaveData)
    //    {
    //        _totalEnemies += item.AmountForWave;
    //        totalenemies += item.AmountForWave;
    //    }
    //
    //    
    //    for (currentenemies = 0; currentenemies < totalenemies;)
    //    {
    //        var holder = data._BaseWaveData[UnityEngine.Random.Range(0, index)];
    //        EnemyList listholder = FindItem(holder.tag);
    //    
    //        if (listholder.Enemys.Count != listholder.MaxAmount && holder.EnemyPrefab != null)
    //        {
    //            enemy = Instantiate(holder.EnemyPrefab);
    //            enemy.transform.position = data._BaseSpawnPoint.position;
    //            currentenemies++;
    //            listholder.Enemys.Add(enemy);
    //    
    //        }
    //        yield return new WaitForSeconds(_spawnInterval);
    //    }
    //    Debug.Log("llmando");
    //    yield return new WaitForSeconds(_spawnInterval);
    //}



    #endregion

    #region Reciving death input
    //private void DeathEnemy(Enemy enemy)
    //{
    //    foreach (var item in EnemyLists)
    //    {
    //        if (item.Enemys.Contains(enemy))
    //        {
    //            item.Enemys.Remove(enemy);
    //            UpdateActiveEnemies();
    //            //UIManager.Instance?.game.WaveProgress(_currentEnemies / _totalEnemies);
    //            break;
    //        }
    //
    //    }
    //
    //
    //    
    //
    //    if (CurrentWave != null && _currentEnemies <= 0)
    //    {
    //        _totalEnemies = 0;
    //        CurrentWave.FinishedWave();
    //    }
    //
    //
    //}
    #endregion

    #region Funcionts
    //public EnemyList FindItem(EnemyTags tag)
    //{
    //
    //    foreach (var item in EnemyLists)
    //    {
    //        if (item.Tag == tag)
    //        {
    //            return item;
    //        }
    //    }
    //    return null;
    //}

    public void UpdateActiveEnemies()
    {
        _currentEnemies = 0;
        foreach (var item in EnemyLists)
        {
            //_currentEnemies += item.Enemys.Count;
        }
    }

    #endregion


    #region Pause
    public void Pause()
    {
        enabled = true;
    }

    public void Resume()
    {
        enabled = false;
    }
    #endregion

    [Serializable]
    public class EnemyList
    {
        //public EnemyTags Tag;
        public int MaxAmount;
        //public List<Enemy> Enemys = new();
    }
    [Serializable]
    public class SpecialWaveData
    {
        public Transform _BaseSpawnPoint;
        public List<EventWave.TypeAmount> _BaseWaveData;
        public GameObject BossRef;
    }
}
