using DataPersistence.Interfaces;

namespace DataPersistence.Services.Encryptors
{
    public class XOREncryptionService : IEncryptor<string, string>
    {
        private readonly string _key;

        public XOREncryptionService(string key)
        {
            _key = key;
        }

        public string Decrypt(string data)
        {
            return XORAlgorithm(data);
        }

        public string Encrypt(string data)
        {
            return XORAlgorithm(data);
        }

        private string XORAlgorithm(string data)
        {
            var result = string.Empty;
            for (var i = 0; i < data.Length; i++) result += (char)(data[i] ^ _key[i % _key.Length]);
            return result;
        }
    }
}