using System.IO;
using UnityEngine;

namespace DataPersistance
{
    public class FileDataPersistanceService
    {
        private readonly string _fileName;
        private readonly string _extension;

        private string DataPath => $"{Application.persistentDataPath}/{_fileName}.{_extension}";

        public FileDataPersistanceService(string fileName, string extension)
        {
            _fileName = fileName;
            _extension = extension;
        }

        public void Save(string data) => File.WriteAllText(DataPath, data);

        public string Load() => File.ReadAllText(DataPath);
    }
}
