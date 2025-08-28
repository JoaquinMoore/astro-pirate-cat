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
    [SerializeField, Tooltip("Sirve para indicar a que oleada el juego esta listo para terminar (nota: esto termina con el ciclo de oleadas y habra que reiniciarlo una ves este la opcion a�adida)")]
    private int _FinalWave;
    [SerializeField, Tooltip("Cuanto tiempo queres que el indicador de la oleada quede en pantalla")]
    private float _animWaitTimer;

    [Header("debug")]
    private int _indexWave = 0;
    private Wave _currentWave;
    private bool Locked;
    private bool onetime;
    private bool freemode;
    private SpawnWavePool _wave;

    public static int IndexWave;
    private float _visualtimer;

    #region Unity Functions
    void Start()
    {
        _wave = GetComponentInChildren<SpawnWavePool>();
        GameManager._wave = this;
        ReciveSpawns.AddListener(ReciveList);
        FinishedWave.AddListener(WaveFinished);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        //GameManager.Instance._PauseGame.Add(this);
        IndexWave = _indexWave;
        StartCoroutine(WaveWaitTimer());
    }

    void Update()
    {

    }
    #endregion

    #region Wave Stuff
    public void StartWave()
    {
        _indexWave++;
        IndexWave = _indexWave;
        _startWave = true;
        _wave.StartSpecial();
        UIManager.MenuManager.SwichMenuState(2, true);
        StartCoroutine(AnimTimer());
        //AudioManager.Instance.Play(SoundName.Alert_Wave.ToString());
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

        _visualtimer = 0;
        _startWave = false;
        _Wave.Clear();
        _Wave = new List<Wave>(_currentWave.WaveChilds);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        StartCoroutine(WaveWaitTimer());

    }


    public void Freemode()
    {
        freemode = true;
        WaveFinished();
    }

    public IEnumerator AnimTimer()
    {
        yield return new WaitForSeconds(_animWaitTimer);
        UIManager.MenuManager.SwichMenuState(2,false);
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


    public float WaveProgress()
    {
        float progress = 0;
        if (_startWave)
            progress = _wave.WaveProgress();
        else
        {
            _visualtimer += Time.deltaTime;
            progress = Mathf.Clamp(_visualtimer / _currentWave.TimeForWave, 0, 1f);
        }



        return progress;
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

        }

        onetime = true;
    }
    #endregion
}

