using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour//, IPause
{
    public static UnityEvent<List<SpawnWavePool.TypeAmount>> ReciveSpawns = new();
    public static UnityEvent FinishedWave = new();
    public static bool _startWave;

    [SerializeField] List<Wave> _Wave = new();
    [SerializeField, Tooltip("Sirve para indicar a que oleada el juego esta listo para terminar (nota: esto termina con el ciclo de oleadas y habra que reiniciarlo una ves este la opcion añadida)")]
    private int _FinalWave;

    [field: SerializeField] public Material MatWave { get; private set; }

    [Header("debug")]
    [field: SerializeField] private int _indexWave = 0;
    [field: SerializeField] private Wave _currentWave;
    private bool Locked;
    private bool onetime;
    private bool freemode;

    [SerializeField] private SpawnWavePool _wave;

    public static int IndexWave;


    #region Unity Functions
    void Start()
    {
        _wave = GetComponentInChildren<SpawnWavePool>();

        ReciveSpawns.AddListener(ReciveList);
        FinishedWave.AddListener(WaveFinished);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        //_currentWave.Reset();
        //GameManager.Instance._PauseGame.Add(this);
        //MatWave.SetInt("_Actived", 1);
        IndexWave = _indexWave;
        StartCoroutine(WaveWaitTimer());
    }

    void Update()
    {
        if (Locked) return;

        //_currentWave.Event();
    }
    #endregion

    #region Wave Stuff
    public void StartWave()
    {
        _indexWave++;
        IndexWave = _indexWave;
        _startWave = true;
        _wave.StartSpecial();
        //MatWave.SetInt("_Actived", 0);
        //StartCoroutine(UIManager.Instance?.ChangedWave(_indexWave));
        //
        //AudioManager.Instance.Play(SoundName.Alert_Wave.ToString());
        //EnemySpawner.StartWave?.Invoke(this);

        _currentWave.Event();

    }

    public void WaveFinished()
    {
        if (_currentWave.WaveChilds.Count == 0)
            return;

        if (_indexWave >= _FinalWave && freemode == false)
        {
            GameManager.Instance.WinState();
            return;
        }


        _startWave = false;
        //MatWave.SetInt("_Actived", 1);
        _Wave.Clear();
        _Wave = new List<Wave>(_currentWave.WaveChilds);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        //_currentWave.Reset();
        StartCoroutine(WaveWaitTimer());

    }


    public void Freemode()
    {
        freemode = true;
        WaveFinished();
    }

    public IEnumerator WaveWaitTimer()
    {
        yield return new WaitForSeconds(_currentWave.TimeForWave);
        StartWave();
    }

    public void ReciveList(List<SpawnWavePool.TypeAmount> Spawns)
    {
        _wave.StartSpawning(Spawns);
    }

    #endregion

    #region Pause
    public void Pause()
    {
        enabled = false;
    }

    public void Resume()
    {
        enabled = true;
    }
    #endregion

    #region Skip Wave
    public void SkipWait()
    {
        if (_currentWave != null)
        {
            StopAllCoroutines();
            StartWave();
        }
    }

    #endregion
    #region Tutorial Shenanigans
    public void Unlock()
    {
        if (!Locked)
            return;
        Locked = false;

    }
    public void Lock()
    {
        Locked = true;
    }


    public void SkipFirstWave()
    {
        if(!onetime)
        {
            var newWave = _currentWave.WaveChilds[Random.Range(0, _currentWave.WaveChilds.Count)];
            _Wave = new List<Wave>(_currentWave.WaveChilds);
            _currentWave = newWave;

            _currentWave.Reset();
        }

        onetime = true;
    }
    #endregion
}

