using System.Linq;
using DataPersistence.Interfaces;
using DataPersistence.Services.DataPersistors;
using DataPersistence.Services.Encryptors;
using DataPersistence.Services.Serializers;
using DataPersistence.Visitors;
using UnityEngine;
using Utility;

namespace DataPersistence
{
    public class DataPersistanceManager : SingletonMono<DataPersistanceManager>
    {
        private const string ENCRYPTED_KEY = "password";
        private const string SAVE_DATA_NAME = "data";
        public const string EXTESION_DATA_NAME = "dat";

        [SerializeField] private bool _encrypted;

        private readonly IEncryptor<string, string> _encryptor = new XOREncryptionService(ENCRYPTED_KEY);

        private readonly FileDataPersistenceService _fileDataHandler = new(SAVE_DATA_NAME, EXTESION_DATA_NAME);
        private readonly IPersistor<GameData> _loadVisitor = new LoaderVisitor();
        private readonly IPersistor<GameData> _saveVisitor = new SaverVisitor();
        private readonly ISerializer _serializer = new JsonSerializationService();
        private GameData _gameData = new();

        public string CurrentPath { get; set; }

        /// <summary>
        ///     Obtiene los datos de todos los objetos que se pueden guarda y modifica el archivo de guardado en el disco.
        /// </summary>
        public void Save()
        {
            GetObjectsData();
            var data = _serializer.Serialize(_gameData);
            if (_encrypted) data = _encryptor.Encrypt(data);
            _fileDataHandler.Save(data);
            Debug.Log("Se guardaron los datos.");
        }

        public void Load()
        {
            var data = _fileDataHandler.Load();
            if (_encrypted) data = _encryptor.Decrypt(data);
            _gameData = _serializer.Deserialize<GameData>(data);
            SetObjectsData();
            Debug.Log("Se cargaron los datos.");
        }

        public void GetObjectsData()
        {
            VisitSaveableObjects(_saveVisitor);
        }

        public void SetObjectsData()
        {
            VisitSaveableObjects(_loadVisitor);
        }

        private void VisitSaveableObjects(IPersistor<GameData> visitor)
        {
            visitor.GameData = _gameData;
            foreach (var item in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPersistence>())
                item.Persist(visitor);
            _gameData = visitor.GameData;
        }
    }
}