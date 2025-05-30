namespace DataPersistence.Interfaces
{
    public interface IPersistence<T> : IPersistence
    {
        public T Data { get; set; }
        public string ID { get; set; }
    }

    public interface IPersistence
    {
        void Persist(IPersistor<GameData> persistor);
    }
}