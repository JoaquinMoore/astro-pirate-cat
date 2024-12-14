using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour//, IPause
{
    [SerializeField] private List<InterestPoints> AllPoints = new();
    [SerializeField,Tooltip("el tiempo entre cada spawn")]
    private float _spawnInterval;
    [SerializeField, Tooltip("que tan grande queres que se vean las sonas en el mapa")]
    private float _MapVisualArea;

    private Dictionary<GameObject, int> ItemList = new();
    private WaveSpawnItems WaveData;

    public static UnityEvent<WaveSpawnItems> StartItemWave = new();

    private int _TotalAmount;
    private int _CurrentAmount;
    private int Count;

    #region Unity Functions
    void Start()
    {
        StartItemWave.AddListener(ReciveData);
        var points = gameObject.GetComponentsInChildren<Transform>();
        foreach (var item in points)
        {
            if (item != transform) //porque hice esto? porque "GetComponentsInChildren" aparentemente tambien hagarra su propia referencia en ciertas ocasiones nose porque pero lo hace y lo odio
            {
                InterestPoints holder = new();
                holder.Point = item.transform;
                holder.MapSpriteRef = item.gameObject.GetComponent<SpriteRenderer>();
                holder.MapSpriteRef.color = new Color(0, 0, 0, 0);
                holder.ID = Count;
                Count++;
                AllPoints.Add(holder);
            }
        }
    }
    #endregion

    #region Recive Data
    public void ReciveData(WaveSpawnItems data)
    {
        WaveData = data;
        foreach (var item in WaveData._Items)
        {
            if (!ItemList.ContainsKey(item.Entety))
                ItemList.Add(item.Entety, 0);
        }
            
        StartCoroutine(SpawnItems());
    }
    #endregion

    #region Spawn Items
    IEnumerator SpawnItems()
    {

        int AmountOfSpawns = WaveData._Items.Count;

        foreach (var item in WaveData._Items)
            _TotalAmount += item.Amount;

        while (_CurrentAmount != _TotalAmount)
        {
            GameObject SpawnHolder;
            var holder = WaveData._Items[Random.Range(0, AmountOfSpawns)];
            int IntHolder = 0;
            if (!holder.PlaceOnID)
                IntHolder = Random.Range(0, AllPoints.Count);
            else
                IntHolder = holder.PlaceID;


            if (ItemList[holder.Entety] == holder.Amount)
            {
                yield return null;
            }
            else
            {
                float floatHolder = Random.Range(1, holder.MaxAreaOfSpawn);
                Vector3 location = AllPoints[IntHolder].Point.position + (Vector3)(Random.insideUnitCircle).normalized * holder.Size * floatHolder;

                var colider = Physics2D.OverlapCircle(location, holder.Size);
                if (colider == null)
                {
                    SpawnHolder = Instantiate(holder.Entety);
                    SpawnHolder.transform.position = location;
;
                    _CurrentAmount++;
                    ItemList[holder.Entety]++;

                    AllPoints[IntHolder].Objetcs.Add(SpawnHolder);
                    AllPoints[IntHolder].MapSpriteRef.color = holder.MapColor;
                   
                }
                yield return new WaitForSeconds(_spawnInterval);
            }
        }
        foreach (var item in AllPoints)
        {
            if (item.Objetcs.Count > 0 && !item.Added)
            {
                //MapTransformManager.AddTransform.Invoke(item.Point);
                item.Added = true;
            }
        }

        ItemList.Clear();
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

    [System.Serializable]
    class InterestPoints
    {
        public Transform Point;
        public SpriteRenderer MapSpriteRef;
        public List<GameObject> Objetcs = new();
        public float MaxDistance;
        public int ID;
        [HideInInspector] public bool Added;
    }
}
