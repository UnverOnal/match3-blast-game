using GamePlay.Board;
using Services.PoolingService;
using VContainer;

namespace GamePlay.CellManagement
{
    public class CellCreator
    {
        private readonly BoardModel _boardModel;
        private readonly ObjectPool<Block> _blockPool;
        private readonly ObjectPool<Obstacle> _obstaclePool;

        [Inject]
        public CellCreator(IPoolService poolService, BoardModel boardModel)
        {
            _boardModel = boardModel;
            _blockPool = poolService.GetPoolFactory().CreatePool(() => new Block());
            _obstaclePool = poolService.GetPoolFactory().CreatePool(() => new Obstacle());
        }

        public void AddCell(CellCreationData cellCreationData)
        {
            var cellType = cellCreationData.levelCellData.cellType;
            var cell = GetCell(cellType);
            cell.SetData(cellCreationData);
            _boardModel.AddCell(cell);
        }

        public void RemoveCell(Cell cell)
        {
            _boardModel.RemoveCell(cell);
            cell.Reset();
            ReturnCell(cell);
        }

        private Cell GetCell(CellType cellType)
        {
            if (cellType == CellType.Obstacle)
                return _obstaclePool.Get();

            return _blockPool.Get();
        }

        private void ReturnCell(Cell cell)
        {
            if (cell.GetType() == typeof(Obstacle))
                _obstaclePool.Return((Obstacle)cell);
            else
                _blockPool.Return((Block)cell);
        }
    }
}