using System;

namespace _UTILITY.PoolManager
{
    public interface IPoolable<out T>
    {
        public event Action<T> OnRelease;
    }
}