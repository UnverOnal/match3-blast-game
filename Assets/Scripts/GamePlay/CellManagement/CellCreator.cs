using GamePlay.Board;
using Services.PoolingService;
using VContainer;

namespace GamePlay.CellManagement
{
    public class CellCreator
    {
        private readonly BoardModel _boardModel;
        private readonly ObjectPool<Cell> _cellPool;

        [Inject]
        public CellCreator(IPoolService poolService, BoardModel boardModel)
        {
            _boardModel = boardModel;
            _cellPool = poolService.GetPoolFactory().CreatePool(() => new Cell());
        }
        
        public void AddCell(CellData cellData)
        {
            var cell = _cellPool.Get();
            cell.SetCellData(cellData);
            _boardModel.AddCell(cell);
        }

        public void RemoveCell(Cell cell)
        {
            _boardModel.RemoveCell(cell);
            cell.Reset();
            _cellPool.Return(cell);
        }
    }
}
