namespace DataPersistance
{
    public interface IPersistance<T> : IPersistance
    {
        public T Data { get; set; }
        public string ID { get; set; }
    }

    public interface IPersistance
    {
        void Persist(IPersistor<GameData> persistor);
    }
}
