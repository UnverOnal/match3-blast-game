using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCounter;
using UnityEngine;

namespace GamePlay.Board
{
    public class Board
    {
        public Cell[,] cells;
        private readonly Dictionary<GameObject, Cell> _cellsMap;

        public Board()
        {
            _cellsMap = new Dictionary<GameObject, Cell>();
        }

        public void SetBoardSize(BoardSize boardSize)
        {
            cells = new Cell[boardSize.x, boardSize.y];
        }

        public void AddCell(Cell cell)
        {
            var location = cell.Location;
            cells[location.x, location.y] = cell;
            _cellsMap.Add(cell.GameObject, cell);
        }

        public void RemoveCell(Cell cell)
        {
            var location = cell.Location;
            cells[location.x, location.y] = null;
            _cellsMap.Remove(cell.GameObject);
        }

        public Cell GetCell(GameObject cellGameObject)
        {
            _cellsMap.TryGetValue(cellGameObject, out var cell);
            return cell;
        }
    }
}