using System.Collections.Generic;
using Board.CellManagement;
using Level;
using UnityEngine;

namespace Board
{
    public class Board
    {
        public Cell[,] cells;
        private readonly Dictionary<GameObject, Cell> _cellsMap;

        public Board()
        {
            _cellsMap = new Dictionary<GameObject, Cell>();
        }
        
        public void SetBoardSize(BoardSize boardSize) => cells = new Cell[boardSize.x, boardSize.y];

        public void AddCell(Cell cell, CellData cellData)
        {
            var location = cellData.location;
            cells[location.x, location.y] = cell;
            _cellsMap.Add(cell.GameObject, cell);
        }

        public void RemoveCell(Cell cell)
        {
            cells[cell.Location.x, cell.Location.y] = null;
            _cellsMap.Remove(cell.GameObject);
        }
        
        public Cell GetCell(GameObject cellGameObject)
        {
            _cellsMap.TryGetValue(cellGameObject, out var cell);
            return cell;
        }
    }
}
