using System.IO;
using UnityEngine;

namespace DataPersistence.Services.DataPersistors
{
    public class FileDataPersistenceService
    {
        private readonly string _extension;
        private readonly string _fileName;

        public FileDataPersistenceService(string fileName, string extension)
        {
            _fileName = fileName;
            _extension = extension;
        }

        private string DataPath => $"{Application.persistentDataPath}/{_fileName}.{_extension}";

        public void Save(string data)
        {
            File.WriteAllText(DataPath, data);
        }

        public string Load()
        {
            return File.ReadAllText(DataPath);
        }
    }
}