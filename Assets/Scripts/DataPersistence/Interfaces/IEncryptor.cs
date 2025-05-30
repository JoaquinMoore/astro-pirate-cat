namespace DataPersistence.Interfaces
{
    public interface IEncryptor<T, R>
    {
        R Encrypt(T data);
        T Decrypt(R data);
    }
}