namespace Services.PoolingService
{
    public interface IPoolService
    {
        ObjectPoolFactory GetPoolFactory();
    }
}
