using System;
using System.Collections.Generic;
using PowerUpManagement.PowerUpTypes;
using Services.PoolingService;
using VContainer;

namespace GamePlay.CellManagement.Creators
{
    public class CellCreator
    {
        [Inject] private readonly CellPrefabCreator _cellPrefabCreator;
        
        private readonly IPoolService _poolService;
        private readonly Dictionary<CellType, ObjectPool<Cell>> _pools;

        [Inject]
        public CellCreator(IPoolService poolService)
        {
            _poolService = poolService;
            _pools = new Dictionary<CellType, ObjectPool<Cell>>();
        }

        public Cell CreateCell(CellCreationData cellCreationData)
        {
            var cellType = cellCreationData.levelCellData.cellType;

            var cell = GetCell(cellType);
            cell.SetData(cellCreationData);
            return cell;
        }

        public void ReturnCell(Cell cell)
        {
            var pool = _pools[cell.CellType];
            pool.Return(cell);
        }

        private void CreatePool(CellType type, out ObjectPool<Cell> pool)
        {
            Func<Cell> creator = type switch
            {
                CellType.Bomb => () => new Bomb(this, _cellPrefabCreator),
                CellType.Rocket => () => new Rocket(this, _cellPrefabCreator),
                CellType.Obstacle => () => new Obstacle(),
                _ => () => new Block()
            };

            pool = _poolService.GetPoolFactory()
                .CreatePool(() => creator?.Invoke());

            _pools.Add(type, pool);
        }
        
        private Cell GetCell(CellType cellType)
        {
            var exist = _pools.TryGetValue(cellType, out var pool);
            if (!exist)
            {
                CreatePool(cellType, out var newPool);
                pool = newPool;
            }

            return pool.Get();
        }
    }
}