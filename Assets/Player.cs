using DataPersistance;
using System;
using UnityEngine;

public class Player : MonoBehaviour, IPersistance<Player.Data>
{
    //[field: SerializeField]
    //public string ID { get; private set; }
    public string ID { get; set; }
    Data IPersistance<Data>.Data
    {
        get
        {
            _data.position = transform.position;
            return _data;
        }
        set
        {
            _data = value;
            transform.position = _data.position;
        }
    }

    //Data IPersistance<Data>.Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField]
    private Data _data;

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = Guid.NewGuid().ToString();
    }

    public void Persist(IPersistor<GameData> persistor)
    {
        persistor.Accept(this);
    }

    [Serializable]
    public struct Data
    {
        public string name;
        public Vector3 position;
        public int health;
    }
}
