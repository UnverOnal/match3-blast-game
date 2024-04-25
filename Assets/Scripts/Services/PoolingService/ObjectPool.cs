using System;
using System.Collections.Generic;

namespace Services.PoolingService
{
    public class ObjectPool<T>
    {
        private readonly Queue<T> _pool;
        
        private readonly Func<T> _objectCreator;
        
        private readonly bool _canExpand;
        
        private readonly int _maxSize;
        private int _counter;

        public ObjectPool(Func<T> objectCreator, bool canExpand, int maxSize)
        {
            _pool = new Queue<T>();
            _objectCreator = objectCreator;
            _canExpand = canExpand;
            _maxSize = maxSize;
        }

        public T Get()
        {
            lock (this)
            {
                if (_pool.Count > 0)
                {
                    _counter--;
                    return _pool.Dequeue();
                }

                if (_counter < _maxSize || _canExpand)
                {
                    _counter++;
                    return _objectCreator.Invoke();
                }

                throw new Exception("Pool is full");
            }
        }

        public void Return(T objectCreated)
        {
            lock (this)
            {
                _counter++;
                _pool.Enqueue(objectCreated);
            }
        }
    }
}