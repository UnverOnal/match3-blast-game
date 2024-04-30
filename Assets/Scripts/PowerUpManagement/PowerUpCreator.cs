using System;
using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCreation;
using PowerUpManagement.PowerUpTypes;
using Services.PoolingService;

namespace PowerUpManagement
{
    public class PowerUpCreator
    {
        private readonly IPoolService _poolService;
        private readonly BoardCreationData _boardCreationData;
        private readonly Dictionary<CellType, ObjectPool<PowerUp>> _pools;

        public PowerUpCreator(IPoolService poolService, BoardCreationData boardCreationData)
        {
            _poolService = poolService;
            _boardCreationData = boardCreationData;
            _pools = new Dictionary<CellType, ObjectPool<PowerUp>>();
        }

        public PowerUp GetPowerUp(CellType type)
        {
            var exist = _pools.TryGetValue(type, out var pool);
            if (!exist)
            {
                CreatePool(type, out var newPool);
                pool = newPool;
            }

            return pool.Get();
        }

        public void ReturnPowerUp(CellType type, PowerUp powerUp)
        {
            var pool = _pools[type];
            pool.Return(powerUp);
        }

        private void CreatePool(CellType type, out ObjectPool<PowerUp> pool)
        {
            Func<PowerUp> powerUp = type switch
            {
                CellType.Bomb => () => new Bomb(),
                CellType.Rocket => () => new Rocket(),
                _ => null
            };
            
            pool = _poolService.GetPoolFactory()
                .CreatePool(() => powerUp?.Invoke());

            _pools.Add(type, pool);
        }
    }
}