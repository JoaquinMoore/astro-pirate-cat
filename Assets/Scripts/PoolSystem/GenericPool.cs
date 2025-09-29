using System;
using UnityEngine;
using UnityEngine.Pool;

namespace WeaponSystem
{
    public class GenericPool<T> where T : Component
    {
        private Func<T> _creationMethod;
        private Action<T> _actionOnGet;
        private Action<T> _actionOnRelease;
        private Action<T> _actionOnDestroy;
        private bool _collectionCheck = true;
        private int _defaultCapacity = 10;
        private int _maxSize = 10000;


        public GenericPool(Func<T> creationMethod)
        {
            _creationMethod = creationMethod;
        }

        public GenericPool<T> SetActionOnGet(Action<T> actionOnGet)
        {
            _actionOnGet = actionOnGet;
            return this;
        }

        public GenericPool<T> SetActionOnRelease(Action<T> actionOnRelease)
        {
            _actionOnRelease = actionOnRelease;
            return this;
        }

        public GenericPool<T> SetActionOnDestroy(Action<T> actionOnDestroy)
        {
            _actionOnDestroy = actionOnDestroy;
            return this;
        }

        public GenericPool<T> SetCollectionCheck(bool collectionCheck)
        {
            _collectionCheck = collectionCheck;
            return this;
        }

        public GenericPool<T> SetDefaultCapacity(int defaultCapacity)
        {
            _defaultCapacity = defaultCapacity;
            return this;
        }

        public GenericPool<T> SetMaxSize(in int maxSize)
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