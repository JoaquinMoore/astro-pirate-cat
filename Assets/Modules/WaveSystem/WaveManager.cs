using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour//, IPause
{
    public static UnityEvent StartEffect = new();
    public static UnityEvent<List<Wave>> ReciveNewList = new();
    public static bool StartWave;

    [SerializeField] List<Wave> _Wave = new();
    [field: SerializeField] public Material MatWave { get; private set; }

    private int _indexWave = 0;
    private Wave _currentWave;
    private bool Locked;
    private bool onetime;

    public static int IndexWave;

    #region Unity Functions
    void Start()
    {
        StartEffect.AddListener(TriggerEffect);
        ReciveNewList.AddListener(ReciveList);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        _currentWave.Reset();
        //GameManager.Instance._PauseGame.Add(this);
        MatWave.SetInt("_Actived", 1);
        IndexWave = _indexWave;
    }

    void Update()
    {
        if (Locked) return;

        _currentWave.Event();
    }
    #endregion

    #region Reciving Events
    public void TriggerEffect()
    {
        _indexWave++;
        IndexWave = _indexWave;
        StartWave = true;
        MatWave.SetInt("_Actived", 0);
        //StartCoroutine(UIManager.Instance?.ChangedWave(_indexWave));
        //
        //AudioManager.Instance.Play(SoundName.Alert_Wave.ToString());
    }

    public void ReciveList(List<Wave> list)
    {
        if (list.Count == 0)
            return;

        StartWave = false;
        MatWave.SetInt("_Actived", 1);
        _Wave.Clear();
        _Wave = new List<Wave>(list);
        _currentWave = _Wave[Random.Range(0, _Wave.Count)];
        _currentWave.Reset();
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

    #region Skip Waiting
    public void SkipWait()
    {
        if (_currentWave != null)
        {
            _currentWave.SkipTimer();
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

