using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/StandarWave")]
public class StandarWave : WaveBase
{
    [Tooltip("Lista de la cantidad de enemigos por oleada")]
    public List<WaveManager.TypeAmount> AmauntPerWave = new();
    [Tooltip("Lista de eventos que pueden ocurrir en la oleada")]
    public List<WaveEvents> WaveEvents = new();

    public override void Event()
    {

    }



}
