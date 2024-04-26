using System.Collections.Generic;
using Board.BoardCreation;
using Board.CellManagement;
using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace Board
{
    public class BoardPresenter
    {
        [Inject] private BoardModel _boardModel;
        private readonly BoardView _boardView;

        private readonly ObjectPool<CellGroup> _groupPool;

        [Inject]
        public BoardPresenter(BlockCreator blockCreator, IPoolService poolService)
        {
            _boardView = new BoardView(blockCreator);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
        }

        public void OnBlockSelected(GameObject cellGameObject)
        {
            GetGroups();
            if(_boardModel.cellGroups.Count < 1)
                return;
            // var group = _boardModel.GetGroup(cellGameObject);
            // Merge(group);
            // _boardView.Collapse();
            // _boardView.Fill();
            // _boardModel.Update();
        }

        private void GetGroups()
        {
            var board = _boardModel.board;
            _boardModel.cellGroups.Clear();
            var visitedCells = new HashSet<Cell>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    var cell = board[i, j];
                    if(visitedCells.Contains(cell) || cell.CellType == BlockType.Obstacle)
                        continue;
                    
                    var group = _groupPool.Get();
                    group.Add(cell);
                    visitedCells.Add(cell);
                    cell.GetNeighbours(group, board, visitedCells);
                    
                    if (group.IsEmpty)
                    {
                        group.Reset();
                        _groupPool.Return(group);
                    }
                    else
                        _boardModel.AddCellGroup(group); 
                }
            }
        }

        private void Merge(CellGroup group)
        {
            _boardView.Merge(group);
            group.Reset();
            _groupPool.Return(group);
        }

        private void Shuffle()
        {
            
        }
    }
}
