using System.Collections.Generic;
using PowerUpManagement.PowerUpTypes;
using Services.PoolingService;

namespace PowerUpManagement
{
    public class PowerUpCreator
    {
        private readonly IPoolService _poolService;
        private readonly Dictionary<PowerUpType, ObjectPool<PowerUp>> _pools;
        private PowerUpCreationData[] _powerUpCreationDatas;

        public PowerUpCreator(IPoolService poolService, PowerUpCreationData[] powerUpCreationDatas)
        {
            _poolService = poolService;
            _powerUpCreationDatas = powerUpCreationDatas;
            _pools = new Dictionary<PowerUpType, ObjectPool<PowerUp>>();
        }

        public PowerUp GetPowerUp(PowerUpType type)
        {
            var exist = _pools.TryGetValue(type, out var pool);
            if (!exist)
            {
                CreatePool(type, out var newPool);
                pool = newPool;
            }

            return pool.Get();
        }

        public void ReturnPowerUp(PowerUpType type, PowerUp powerUp)
        {
            var pool = _pools[type];
            pool.Return(powerUp);
        }

        private void CreatePool(PowerUpType type, out ObjectPool<PowerUp> pool)
        {
            PowerUp powerUp = type switch
            {
                PowerUpType.Bomb => new Bomb(),
                PowerUpType.Rocket => new Rocket(),
                _ => null
            };

            SetData(type, powerUp);
            
            pool = _poolService.GetPoolFactory()
                .CreatePool(() => powerUp);

            _pools.Add(type, pool);
        }

        private void SetData(PowerUpType type, PowerUp powerUp)
        {
            var data = new PowerUpCreationData();
            for (var i = 0; i < _powerUpCreationDatas.Length; i++)
            {
                if (_powerUpCreationDatas[i].type != type) continue;
                data = _powerUpCreationDatas[i];
                break;
            }
            
            powerUp.SetData(data);
        }
    }
}