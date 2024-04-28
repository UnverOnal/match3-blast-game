using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCounter;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BoardModel
    {
        public readonly List<CellGroup> cellGroups;
        public Cell[,] Cells => _board.cells;

        private readonly ObjectPool<Cell> _cellPool;

        private readonly global::GamePlay.Board.Board _board;

        [Inject]
        public BoardModel(IPoolService poolService)
        {
            _cellPool = poolService.GetPoolFactory().CreatePool(()=> new Cell());
            cellGroups = new List<CellGroup>();

            _board = new global::GamePlay.Board.Board();
        }

        public void SetBoardSize(BoardSize boardSize) => _board.SetBoardSize(boardSize);

        public void AddCell(CellData cellData)
        {
            var cell = _cellPool.Get();
            cell.SetCellData(cellData);
            _board.AddCell(cell, cellData);
        }

        public void RemoveCell(Cell cell)
        {
            _board.RemoveCell(cell);
            cell.Reset();
            _cellPool.Return(cell);
        }

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
