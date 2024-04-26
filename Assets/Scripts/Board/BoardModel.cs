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
        
        private readonly ObjectPool<Cell> _cellPool;

        [Inject]
        public BoardModel(IPoolService poolService)
        {
            _cellPool = poolService.GetPoolFactory().CreatePool(()=> new Cell());
            cellGroups = new List<CellGroup>();
        }

        public void SetBoardSize(BoardSize boardSize) => board = new Cell[boardSize.x, boardSize.y];

        public void AddCell(BoardLocation location, GameObject gameObject)
        {
            var cell = _cellPool.Get();
            cell.SetLocation(location);
            cell.SetGameObject(gameObject);
            board[location.x, location.y] = cell;
        }

        public void AddCellGroup(CellGroup group)
        {
            cellGroups.Add(group);
        }

        public CellGroup GetGroup(GameObject cellGameObject)
        {
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
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            
        }
    }

    public struct BoardLocation
    {
        public readonly int x;
        public readonly int y;

        public BoardLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
