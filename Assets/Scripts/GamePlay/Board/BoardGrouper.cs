using System.Collections.Generic;
using GamePlay.CellManagement;
using Services.PoolingService;

namespace GamePlay.Board
{
    public class BoardGrouper
    {
        private readonly ObjectPool<CellGroup> _groupPool;

        public BoardGrouper(ObjectPool<CellGroup> groupPool)
        {
            _groupPool = groupPool;
        }
        
        public void GroupCells(BoardModel boardModel)
        {
            var board = boardModel.Cells;
            boardModel.cellGroups.Clear();
            var visitedCells = new HashSet<Cell>();

            for (var i = 0; i < board.GetLength(0); i++)
            for (var j = 0; j < board.GetLength(1); j++)
            {
                if (!CanGroup(board[i, j], visitedCells))
                    continue;

                var cell = (Block)board[i, j];
                var group = _groupPool.Get();
                group.AddCell(cell);
                visitedCells.Add(cell);
                cell.GetNeighbours(group, board, visitedCells);

                if (group.IsEmpty)
                {
                    group.Reset();
                    _groupPool.Return(group);
                }
                else
                    boardModel.AddCellGroup(group);
            }
        }

        private bool CanGroup(Cell cell, HashSet<Cell> visitedCells)
        {
            var isEmpty = cell == null;
            var isObstacle = cell?.GetType() == typeof(Obstacle);
            var isVisited = visitedCells.Contains(cell);

            return !isEmpty && !isObstacle && !isVisited;
        }
    }
}
