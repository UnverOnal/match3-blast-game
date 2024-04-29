using System.Collections.Generic;

namespace GamePlay.CellManagement
{
    public class Block : Cell
    {
        public BlockType BlockType { get; private set; }
        
        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (LevelBlockData)cellCreationData.levelCellData;
            BlockType = cellData.type;
        }

        public void GetNeighbours(CellGroup cellGroup, Cell[,] board, HashSet<Cell> visitedCells)
        {
            var (boardWidth, boardHeight) = (board.GetLength(0), board.GetLength(1));
            var (horizontalDimension, verticalDimension) = (new[] { 0, 1, 0, -1 }, new[] { -1, 0, 1, 0 });

            for (int i = 0; i < 4; i++)
            {
                var (x, y) = (Location.x + horizontalDimension[i], Location.y + verticalDimension[i]);
                if (!IsValidLocation(x, y, boardWidth, boardHeight)) continue;
                
                var neighbour = board[x, y];
                if (IsNeighbourValid(neighbour, visitedCells))
                {
                    var blockNeighbour = (Block)neighbour;
                    cellGroup.AddCell(blockNeighbour);
                    visitedCells.Add(blockNeighbour);
                    blockNeighbour.GetNeighbours(cellGroup, board, visitedCells);
                }
                GetDamageableNeighbour(neighbour, cellGroup);
            }
        }
        
        private bool IsValidLocation(int x, int y, int boardWidth, int boardHeight)
        {
            return x >= 0 && x < boardWidth && y >= 0 && y < boardHeight;
        }

        private bool IsNeighbourValid(Cell neighbour, ICollection<Cell> visitedCells)
        {
            var isEmpty = neighbour == null;
            var isSameType = neighbour?.GetType() == typeof(Block) && BlockType == ((Block)neighbour).BlockType;
            var isVisited = visitedCells.Contains(neighbour);

            return !isEmpty && isSameType && !isVisited;
        }

        private void GetDamageableNeighbour(Cell neighbour, CellGroup cellGroup)
        {
            if (neighbour is IDamageable damageable)
                cellGroup.AddDamageable(damageable);
        }
    }
}
