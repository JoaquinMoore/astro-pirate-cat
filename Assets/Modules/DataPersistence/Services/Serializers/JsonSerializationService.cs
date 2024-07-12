using UnityEngine;

namespace DataPersistance
{
    public class JsonSerializationService : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
