using System;
using System.Collections.Generic;
using GamePlay.Board;
using PowerUpManagement.PowerUpTypes;
using Services.PoolingService;
using VContainer;

namespace GamePlay.CellManagement
{
    public class CellCreator
    {
        private readonly IPoolService _poolService;
        private readonly BoardModel _boardModel;
        private readonly Dictionary<CellType, ObjectPool<Cell>> _pools;

        [Inject]
        public CellCreator(IPoolService poolService, BoardModel boardModel)
        {
            _poolService = poolService;
            _boardModel = boardModel;
            _pools = new Dictionary<CellType, ObjectPool<Cell>>();
        }
        
        public Cell GetCell(CellType cellType)
        {
            var exist = _pools.TryGetValue(cellType, out var pool);
            if (!exist)
            {
                CreatePool(cellType, out var newPool);
                pool = newPool;
            }

            return pool.Get();
        }

        public void AddCell(CellCreationData cellCreationData)
        {
            var cellType = cellCreationData.levelCellData.cellType;

            var cell = GetCell(cellType);
            cell.SetData(cellCreationData);
            _boardModel.AddCell(cell);
        }

        public void RemoveCell(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
                RemoveCell(cell);
        }

        public void RemoveCell(Cell cell)
        {
            _boardModel.RemoveCell(cell);
            ReturnCell(cell);
        }

        private void ReturnCell(Cell cell)
        {
            var pool = _pools[cell.CellType];
            pool.Return(cell);
        }

        private void CreatePool(CellType type, out ObjectPool<Cell> pool)
        {
            Func<Cell> creator = type switch
            {
                CellType.Bomb => () => new Bomb(),
                CellType.Rocket => () => new Rocket(),
                CellType.Obstacle => () => new Obstacle(),
                _ => () => new Block()
            };

            pool = _poolService.GetPoolFactory()
                .CreatePool(() => creator?.Invoke());

            _pools.Add(type, pool);
        }
    }
}