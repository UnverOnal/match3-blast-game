using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCounter;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BoardModel
    {
        public readonly List<CellGroup> cellGroups;
        public Cell[,] Cells => _board.cells;
        
        private readonly Board _board;

        public BoardModel()
        {
            cellGroups = new List<CellGroup>();

            _board = new Board();
        }

        public void SetBoardSize(int width, int height) => _board.SetBoardSize(width, height);

        public void AddCell(Cell cell) => _board.AddCell(cell);

        public void RemoveCell(Cell cell) => _board.RemoveCell(cell);

        public void UpdateCellLocation(Cell cell, BoardLocation targetLocation)
        {
            var location = cell.Location;
            _board.cells[location.x, location.y] = null;
            _board.cells[targetLocation.x, targetLocation.y] = cell;
            cell.SetLocation(targetLocation);
        }

        public void AddCellGroup(CellGroup group) => cellGroups.Add(group);
        public void RemoveCellGroup(CellGroup group) => cellGroups.Remove(group);

        public CellGroup GetGroup(GameObject cellGameObject)
        {
            if (cellGroups.Count < 1)
                return null;
            
            var cell = GetCell(cellGameObject);
            
            for (int i = 0; i < cellGroups.Count; i++)
            {
                var group = cellGroups[i];
                if (group.HasCell(cell))
                    return group;
            }

            return null;
        }

        private Cell GetCell(GameObject cellGameObject) => _board.GetCell(cellGameObject);
    }
}
