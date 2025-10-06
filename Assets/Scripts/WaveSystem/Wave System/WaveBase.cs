using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBase : ScriptableObject
{
    [Tooltip("Indica el tiempo de espera para la oleada")]
    public float TimeForWave;
    [Tooltip("La lista de posible proxima oleadas que se podrian ejecutar")]
    public List<WaveBase> WaveChilds = new();

    public virtual void Event() {}
}


