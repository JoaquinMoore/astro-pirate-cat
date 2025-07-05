using UnityEngine;
using UnityEngine.Serialization;

namespace _UTILITY
{
    public abstract class BaseDataWrapperSO<T> : ScriptableObject
    {
        public T value;

        public static implicit operator T(BaseDataWrapperSO<T> wrapper) => wrapper.value;
    }
}