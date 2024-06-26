using System;
using GamePlay.CellManagement;

namespace GamePlay.Board.Steps
{
    public class BoardShuffler
    {
        public void Shuffle(Cell[,] cells)
        {
            var random = new Random();
            var rows = cells.GetLength(0);
            var cols = cells.GetLength(1);

            for (var i = rows - 1; i > 0; i--)
            {
                for (var j = cols - 1; j > 0; j--)
                {
                    var m = random.Next(i + 1);
                    var n = random.Next(j + 1);

                    SwapCells(cells, i, j, m, n);
                }
            }
        }

        private void SwapCells(Cell[,] cells, int row1, int col1, int row2, int col2)
        {
            if (!IsSwapValid(cells[row1, col1], cells[row2, col2]))
                return;

            var tempCell = cells[row1, col1];
            cells[row1, col1] = cells[row2, col2];
            cells[row2, col2] = tempCell;

            tempCell?.SetLocation(new BoardLocation(row2, col2));
            cells[row1, col1]?.SetLocation(new BoardLocation(row1, col1));
        }
        
        private bool IsSwapValid(Cell cell, Cell cellToReplace)
        {
            var isEmpty = cell == null || cellToReplace == null;
            var isBlock = cell?.GetType() != typeof(Obstacle) && cellToReplace?.GetType() != typeof(Obstacle);
            return !isEmpty && isBlock;
        }
    }
}
