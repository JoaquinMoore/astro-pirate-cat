using UnityEngine;
using Utility;
using System.Linq;

namespace DataPersistance
{
    public class DataPersistanceManager : SingletonMono<DataPersistanceManager>
    {
        private const string ENCRYPTED_KEY = "password";
        private const string SAVE_DATA_NAME = "data";
        public const string EXTESION_DATA_NAME = "dat";

        public string CurrentPath { get; set; }

        [SerializeField]
        private bool _encrypted = false;
        private GameData _gameData = new();

        private readonly FileDataPersistanceService _fileDataHandler = new(SAVE_DATA_NAME, EXTESION_DATA_NAME);
        private readonly ISerializer _serializer = new JsonSerializationService();
        private readonly IEncryptor<string, string> _encryptor = new XOREncryptionService(ENCRYPTED_KEY);
        private readonly IPersistor<GameData> _saveVisitor = new SaverVisitor();
        private readonly IPersistor<GameData> _loadVisitor = new LoaderVisitor();

        /// <summary>
        /// Obtiene los datos de todos los objetos que se pueden guarda y modifica el archivo de guardado en el disco.
        /// </summary>
        public void Save()
        {
            GetObjectsData();
            string data = _serializer.Serialize(_gameData);
            if (_encrypted)
            {
                data = _encryptor.Encrypt(data);
            }
            _fileDataHandler.Save(data);
            Debug.Log("Se guardaron los datos.");
        }

        public void Load()
        {
            string data = _fileDataHandler.Load();
            if (_encrypted)
            {
                data = _encryptor.Decrypt(data);
            }
            _gameData = _serializer.Deserialize<GameData>(data);
            SetObjectsData();
            Debug.Log("Se cargaron los datos.");
        }

        public void GetObjectsData() => VisitSaveableObjects(_saveVisitor);

        public void SetObjectsData() => VisitSaveableObjects(_loadVisitor);

        private void VisitSaveableObjects(IPersistor<GameData> visitor)
        {
            visitor.GameData = _gameData;
            foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IPersistance>())
            {
                item.Persist(visitor);
            }
            _gameData = visitor.GameData;
        }
    }
}
