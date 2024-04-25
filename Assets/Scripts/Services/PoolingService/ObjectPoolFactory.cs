using System;

namespace Services.PoolingService
{
    public class ObjectPoolFactory
    {
        public ObjectPool<T> CreatePool<T>(Func<T> creator, bool canExpand = true, int maxSize = 0)
        {
            return new ObjectPool<T>(creator, canExpand, maxSize);
        }
    }
}