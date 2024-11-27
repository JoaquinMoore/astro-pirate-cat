using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NPCtestPool : MonoBehaviour
{
    public SpawnWavePool pool;
    public EnemyTags etag;
    public bool TestDeath;

    protected IObjectPool<NPCtestPool> nPCtestPool;
    public IObjectPool<NPCtestPool> _NPCtestPool { set => nPCtestPool = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TestDeath)
        {
            Death();
        }
    }

    public void GiveRef(SpawnWavePool a, EnemyTags tag)
    {
        pool = a;
        etag = tag;
    }


    public void Death()
    {
        TestDeath = false;
        if (pool != null)
            pool.EnemyDeathCallBack(this, etag);
        nPCtestPool.Release(this);
    }
}
