using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/SpawnWave")]
public class EventWave : Wave
{
    [Tooltip("Lista de la cantidad de enemigos por oleada")]
    public List<TypeAmount> AmauntPerWave = new();
    [Tooltip("Lista de eventos que pueden ocurrir en la oleada")]
    public List<WaveEvents> WaveEvents = new();
    private float _nextWaveTimer = 0;
    private bool _startWave = true;
    [System.Serializable]
    public class TypeAmount
    {
        //public EnemyTags tag;
        //public Enemy EnemyPrefab;
        public int AmountForWave;
    }
    public override void Reset()
    {
        _nextWaveTimer = 0;
        _startWave = true;
    }

    public override void Event()
    {
        if (_startWave == false) return;
        _nextWaveTimer += Time.deltaTime;
        //UIManager.Instance?.game.WaveProgress(_nextWaveTimer / TimeForWave);

        if (_nextWaveTimer >= TimeForWave)
        {
            StartWave();
        }
    }

    public override void SkipTimer()
    {
        _nextWaveTimer = TimeForWave;
    }


    public void StartWave()
    {
        _startWave = false;
        WaveManager.StartEffect?.Invoke();
        //EnemySpawner.StartWave?.Invoke(this);
        if (WaveEvents.Count == 0)
            return;
        foreach (var item in WaveEvents)
        {
            item.Event();
        }
    }

    public override void FinishedWave()
    {
        WaveManager.ReciveNewList?.Invoke(WaveChilds);
    }
}
