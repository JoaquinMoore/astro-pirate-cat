using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    public List<SpawnWavePool.TypeAmount> _currentWave;
    public List<SpawnWavePool.SpawnPoints> _SpawnPoints = new();

    // Start is called before the first frame update
    void Start()
    {
        SpawnWavePool.Sendspawns.AddListener(Call);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Call()
    {
        SpawnWavePool.ReciveSpawns?.Invoke(_currentWave, _SpawnPoints);
    }
}
