namespace DataPersistance
{
    public interface IDataSaver<T, R>
    {
        void Save(T data);
        R Load(T data);
    }
}
