public interface IBrain<T>
{
    void Awake(T controller);
    void Think();
}
