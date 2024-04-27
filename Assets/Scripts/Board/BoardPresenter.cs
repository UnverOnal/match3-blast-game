using System.Collections.Generic;
using System.Linq;
using Board.BoardCreation;
using Board.CellManagement;
using Cysharp.Threading.Tasks;
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

        private bool _canGroup = true;

        [Inject]
        public BoardPresenter(BlockCreator blockCreator, IPoolService poolService)
        {
            _boardView = new BoardView(blockCreator);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
        }

        public async void OnBlockSelected(GameObject selectedBlock)
        {
            if (_canGroup)
            {
                GetGroups();
                _canGroup = false;
            }

            var group = _boardModel.GetGroup(selectedBlock);
            if (group == null)
            {
                _boardView.Shake(selectedBlock.transform, 0.1f, 30f);
                return;
            }

            var bottomBlastedLocations = new List<BoardLocation>(group.GetLocations());
            await Blast(group, selectedBlock);
            _canGroup = true; //It can group if there is a merge

            Collapse(bottomBlastedLocations, _boardModel.Cells);
            Fill();
            // _boardModel.Update();
        }

        private void GetGroups()
        {
            var board = _boardModel.Cells;
            _boardModel.cellGroups.Clear();
            var visitedCells = new HashSet<Cell>();

            for (var i = 0; i < board.GetLength(0); i++)
            for (var j = 0; j < board.GetLength(1); j++)
            {
                var cell = board[i, j];
                if (visitedCells.Contains(cell) || cell.CellType == BlockType.Obstacle)
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

        private async UniTask Blast(CellGroup group, GameObject selectedBlock)
        {
            await _boardView.Blast(group, selectedBlock);
            var groupCells = group.cells;
            foreach (var cellPair in groupCells)
                _boardModel.RemoveCell(cellPair.Value);
            group.Reset();
            _groupPool.Return(group);
        }

        private void Collapse(IEnumerable<BoardLocation> blastedLocations, Cell[,] cells)
        {
            foreach (var location in blastedLocations)
            {
                var yLocation = location.y;
                for (var j = location.y + 1; j < cells.GetLength(1); j++)
                {
                    var cell = cells[location.x, j];
                    if (cell == null)
                        continue;
                    if (cell.CellType == BlockType.Obstacle)
                    {
                        yLocation = cell.Location.y + 1;
                        continue;
                    }

                    _boardModel.UpdateCellLocation(cell, new BoardLocation(location.x, yLocation++));
                    _boardView.Collapse(cell, j);
                }
            }
        }

        private void Fill()
        {
            _boardView.Fill();
        }

        private void Shuffle()
        {
        }
    }
}