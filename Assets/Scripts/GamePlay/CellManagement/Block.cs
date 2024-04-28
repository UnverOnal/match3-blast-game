using System.Collections.Generic;
using Level.LevelCounter;

namespace GamePlay.CellManagement
{
    public class Block : CellBase
    {
        public void GetNeighbours(CellGroup cellGroup, Cell[,] board, HashSet<Cell> visitedCells)
        {
            int[] horizontalDimension = { 0, 1, 0, -1 };
            int[] verticalDimension = { -1, 0, 1, 0 };
            
            var boardWidth = board.GetLength(0);
            var boardHeight = board.GetLength(1);

            //Traverse neighbours
            for (int i = 0; i < 4; i++)
            {
                var x = Location.x + horizontalDimension[i];
                var y = Location.y + verticalDimension[i];

                var locationExist = x >= 0 && x < boardWidth && y >= 0 && y < boardHeight;
                if (locationExist)
                {
                    var neighbour = board[x, y];
                    if (neighbour == null || CellType != neighbour.CellType || visitedCells.Contains(neighbour))
                        continue;

                    //Add cell group if it is same type
                    cellGroup.Add(neighbour);
                    visitedCells.Add(neighbour);
                    neighbour.GetNeighbours(cellGroup, board, visitedCells);
                }
            }
        }
    }
}
