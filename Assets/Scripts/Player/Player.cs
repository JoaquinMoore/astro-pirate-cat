using System;
using DataPersistence;
using DataPersistence.Interfaces;
using UnityEngine;

public class Player : MonoBehaviour, IPersistence<Player.Data>
{
    //Data IPersistance<Data>.Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField] private Data _data;

    //[field: SerializeField]
    //public string ID { get; private set; }
    public string ID { get; set; }

    Data IPersistence<Data>.Data
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

    public void Persist(IPersistor<GameData> persistor)
    {
        persistor.Accept(this);
    }

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = Guid.NewGuid().ToString();
    }

    [Serializable]
    public struct Data
    {
        public string name;
        public Vector3 position;
        public int health;
    }
}