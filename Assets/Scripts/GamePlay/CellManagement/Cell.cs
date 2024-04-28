using System.Collections.Generic;
using Level.LevelCounter;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public class Cell
    {
        public Vector3 Position => GameObject.transform.position;
        public Vector2 Extents => _sprite.bounds.extents * (Vector2)GameObject.transform.localScale;
        
        public GameObject GameObject { get; private set; }
        public CellType CellType { get; private set; }

        public BoardLocation Location { get; private set; }
        
        private Sprite _sprite;

        public void SetCellData(CellData cellData)
        {
            SetLocation(cellData.location);
            CellType = cellData.cellType;
            
            GameObject = cellData.gameObject;
            _sprite = GameObject.GetComponent<SpriteRenderer>().sprite;
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
        
        public void Reset()
        {
            GameObject = null;
            Location = new BoardLocation();
            _sprite = null;
        }

        public void SetLocation(BoardLocation boardLocation)
        {
            Location = boardLocation;
        }
    }
}
