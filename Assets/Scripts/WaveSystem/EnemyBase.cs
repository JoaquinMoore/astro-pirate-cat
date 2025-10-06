using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private List<WaveManager.TypeAmount> _currentWave;
    [SerializeField] private List<Transform> _SpawnPoints = new();

    void Start()
    {
        WaveManager.Sendspawns.AddListener(Call);
    }
    public void Call()
    {
        WaveManager.ReciveSpawns?.Invoke(_currentWave, _SpawnPoints);
    }
}
