namespace DataPersistance
{
    public interface IPersistor<T>
    {
        public T GameData { get; set; }
        void Accept(IPersistance<Player.Data> obj);
    }
}
