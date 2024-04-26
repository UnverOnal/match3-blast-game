using System.Collections.Generic;
using Board.CellManagement;
using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace Board
{
    public class BoardModel
    {
        public readonly List<CellGroup> cellGroups;
        
        public Cell[,] board;
        private readonly Dictionary<GameObject, Cell> _boardMap; 
        
        private readonly ObjectPool<Cell> _cellPool;

        [Inject]
        public BoardModel(IPoolService poolService)
        {
            _cellPool = poolService.GetPoolFactory().CreatePool(()=> new Cell());
            cellGroups = new List<CellGroup>();
            _boardMap = new Dictionary<GameObject, Cell>();
        }

        public void SetBoardSize(BoardSize boardSize) => board = new Cell[boardSize.x, boardSize.y];

        public void AddCell(CellData cellData)
        {
            var cell = _cellPool.Get();
            cell.SetCellData(cellData);
            var location = cellData.location;
            board[location.x, location.y] = cell;
            _boardMap.Add(cell.GameObject, cell);
        }

        public void RemoveCell(Cell cell)
        {
            board[cell.Location.x, cell.Location.y] = null;
            _boardMap.Remove(cell.GameObject);
            cell.Reset();
            _cellPool.Return(cell);
        }

        public void AddCellGroup(CellGroup group)
        {
            cellGroups.Add(group);
        }

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
        
        private Cell GetCell(GameObject cellGameObject)
        {
            _boardMap.TryGetValue(cellGameObject, out var cell);
            return cell;
        }
    }
}
