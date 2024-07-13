namespace DataPersistance
{
    public interface ISaveable<T>
    {
        void Save(ref T data);
        void Load(T data);
    }
}
