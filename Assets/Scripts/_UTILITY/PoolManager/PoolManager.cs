using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace _UTILITY.PoolManager
{
    public static class PoolManager
    {
        private static readonly Dictionary<Type, object> s_pools = new();

        private static IObjectPool<T> GetPool<T>() where T : class, IPoolable<T>, new()
        {
            var type = typeof(T);

            if (!s_pools.ContainsKey(type))
            {
                s_pools[type] = new ObjectPool<T>(
                    () => new T(),
                    obj => obj.OnRelease += Release,
                    obj => obj.OnRelease -= Release
                );
            }

            return s_pools[type] as IObjectPool<T>;
        }

        public static T Get<T>() where T : class, IPoolable<T>, new() => GetPool<T>().Get();

        public static void Release<T>(T task) where T : class, IPoolable<T>, new() => GetPool<T>().Release(task);
    }
}