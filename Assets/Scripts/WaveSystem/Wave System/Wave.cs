using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : ScriptableObject
{
    [Tooltip("Indica el tiempo de espera para la oleada")]
    public float TimeForWave;
    [Tooltip("La lista de posible proxima oleadas que se podrian ejecutar")]
    public List<Wave> WaveChilds = new();

    public virtual void Event() {}
}


