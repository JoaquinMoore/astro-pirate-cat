using System;
using UnityEngine;
using UnityEngine.Pool;

namespace WeaponSystem
{
    public class TempPool<T> where T : Component
    {
        #region oldpool
        //bulletsPool = new ObjectPool<BaseBullet>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,true, 30, 300);

        #endregion

        private Func<T> _creationMethod;
        private Action<T> _actionOnGet;
        private Action<T> _actionOnRelease;
        private Action<T> _actionOnDestroy;
        private bool _collectionCheck = true;
        private int _defaultCapacity = 10;
        private int _maxSize = 10000;


        public TempPool(Func<T> creationMethod)
        {
            _creationMethod = creationMethod;
        }

        public TempPool<T> SetActionOnGet(Action<T> actionOnGet)
        {
            _actionOnGet = actionOnGet;
            return this;
        }

        public TempPool<T> SetActionOnRelease(Action<T> actionOnRelease)
        {
            _actionOnRelease = actionOnRelease;
            return this;
        }

        public TempPool<T> SetActionOnDestroy(Action<T> actionOnDestroy)
        {
            _actionOnDestroy = actionOnDestroy;
            return this;
        }

        public TempPool<T> SetCollectionCheck(bool collectionCheck)
        {
            _collectionCheck = collectionCheck;
            return this;
        }

        public TempPool<T> SetDefaultCapacity(int defaultCapacity)
        {
            _defaultCapacity = defaultCapacity;
            return this;
        }

        public TempPool<T> SetMaxSize(in int maxSize)
        {
            _maxSize = maxSize;
            return this;
        }

        public IObjectPool<T> Build()
        {
            return new ObjectPool<T>(
                _creationMethod,
                _actionOnGet,
                _actionOnRelease,
                _actionOnDestroy,
                _collectionCheck,
                _defaultCapacity,
                _maxSize
            );
        }
    }
}