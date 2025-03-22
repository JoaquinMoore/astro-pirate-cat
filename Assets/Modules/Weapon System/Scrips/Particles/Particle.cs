using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Particle : MonoBehaviour
{
    protected IObjectPool<Particle> _particlePool;
    public IObjectPool<Particle> PaticlePool { set => _particlePool = value; }


    private void OnDisable()
    {
        if (_particlePool != null)
            _particlePool.Release(this);
    }

}
