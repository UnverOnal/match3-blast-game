namespace Services.PoolingService
{
    public class PoolService : IPoolService
    {
        public ObjectPoolFactory GetPoolFactory()
        {
            return new ObjectPoolFactory();
        }
    }
}