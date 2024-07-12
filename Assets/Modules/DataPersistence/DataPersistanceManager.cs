using System;
using System.Linq;
using UnityEngine;

namespace DataPersistance
{
    public class DataPersistanceManager : MonoBehaviour
    {
        private const string ENCRYPTED_KEY = "astrocat";

        [SerializeField]
        private GameData _gameData = new();
        [SerializeField]
        private bool _encrypted = false;

        private FileDataPersistanceService _fileDataHandler = new("data", "dat");
        private ISerializer _serializer = new JsonSerializationService();
        private IEncryptor<string, string> _encryptor = new XOREncryptionService(ENCRYPTED_KEY);

        [ContextMenu("Save")]
        public void Save()
        {
            ApplyToSaveable(item => item.Save(ref _gameData));
            string data = _serializer.Serialize(_gameData);
            if (_encrypted)
            {
                data = _encryptor.Encrypt(data);
            }
            _fileDataHandler.Save(data);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            string data = _fileDataHandler.Load();
            if (_encrypted)
            {
                _encryptor.Decrypt(data);
            }
            _gameData = _serializer.Deserialize<GameData>(data);
            ApplyToSaveable(item => item.Load(_gameData));
        }

        private void ApplyToSaveable(Action<ISaveable<GameData>> action)
        {
            foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<ISaveable<GameData>>())
            {
                action(item);
            }
        }
    }
}
