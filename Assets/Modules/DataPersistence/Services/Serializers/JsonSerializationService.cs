using Newtonsoft.Json;

namespace DataPersistance
{
    public class JsonSerializationService : ISerializer
    {
        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(
            obj,
            Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
        );

        public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
    }
}
