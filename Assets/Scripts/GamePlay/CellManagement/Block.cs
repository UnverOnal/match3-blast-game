using System.Collections.Generic;

namespace GamePlay.CellManagement
{
    public class Block : Cell
    {
        public BlockType BlockType { get; private set; }
        
        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (BlockData)cellCreationData.cellData;
            BlockType = cellData.type;
        }

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
                // if (!locationExist) continue;
                if (!locationExist || !IsNeighbourValid(board[x, y], visitedCells))
                    continue;

                var neighbour = (Block)board[x, y];
                    
                //Add cell group if it is same type
                cellGroup.Add(neighbour);
                visitedCells.Add(neighbour);
                neighbour.GetNeighbours(cellGroup, board, visitedCells);
            }
        }

        private bool IsNeighbourValid(Cell neighbour, ICollection<Cell> visitedCells)
        {
            var isEmpty = neighbour == null;
            var isSameType = neighbour?.GetType() == typeof(Block) && BlockType == ((Block)neighbour).BlockType;
            var isVisited = visitedCells.Contains(neighbour);

            return !isEmpty && isSameType && !isVisited;
        }
    }
}
