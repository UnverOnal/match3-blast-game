using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace Board
{
    public class BoardModel
    {
        private readonly ObjectPool<Cell> _cellPool;
        public Cell[,] board;

        [Inject]
        public BoardModel(IPoolService poolService)
        {
            _cellPool = poolService.GetPoolFactory().CreatePool(()=> new Cell());
        }

        public void SetBoardSize(BoardSize boardSize) => board = new Cell[boardSize.x, boardSize.y];

        public void AddCell(BoardLocation location, GameObject gameObject)
        {
            var cell = _cellPool.Get();
            cell.SetLocation(location);
            cell.SetGameObject(gameObject);
            board[location.x, location.y] = cell;
        }
    }

    public struct BoardLocation
    {
        public readonly int x;
        public readonly int y;

        public BoardLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
