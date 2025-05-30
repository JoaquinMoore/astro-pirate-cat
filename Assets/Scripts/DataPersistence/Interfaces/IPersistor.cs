namespace DataPersistence.Interfaces
{
    public interface IPersistor<T>
    {
        public T GameData { get; set; }
        void Accept(IPersistence<Player.Data> obj);
    }
}