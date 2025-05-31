using UnityEngine;

namespace _UTILITY
{
    public abstract class BaseDataWrapperSO<T> : ScriptableObject
    {
        [SerializeField] private T _data;

        public static implicit operator T(BaseDataWrapperSO<T> data)
        {
            return data._data;
        }
    }
}