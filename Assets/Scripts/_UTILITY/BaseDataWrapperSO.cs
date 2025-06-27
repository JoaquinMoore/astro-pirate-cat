using UnityEngine;

namespace _UTILITY
{
    public abstract class BaseDataWrapperSO<T> : ScriptableObject
    {
        public T data;

        public static implicit operator T(BaseDataWrapperSO<T> data) => data.data;
    }
}