using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/SpawnWave")]
public class EventWave : Wave
{
    [Tooltip("Lista de la cantidad de enemigos por oleada")]
    public List<SpawnWavePool.TypeAmount> AmauntPerWave = new();
    [Tooltip("Lista de eventos que pueden ocurrir en la oleada")]
    public List<WaveEvents> WaveEvents = new();

    public override void Event()
    {
        WaveManager.ReciveSpawns?.Invoke(AmauntPerWave);
        if (WaveEvents.Count == 0)
            return;
        foreach (var item in WaveEvents)
        {
            item.Event();
        }
    }



}
