using System;
using System.Collections.Generic;
using Board.BoardCreation;
using Board.CellManagement;
using Cysharp.Threading.Tasks;
using GameManagement;
using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace Board
{
    public class BoardPresenter : IInitializable, IDisposable
    {
        [Inject] private BoardModel _boardModel;
        private readonly BoardView _boardView;

        private readonly ObjectPool<CellGroup> _groupPool;

        private bool _canGroup = true;

        [Inject]
        public BoardPresenter(BlockCreator blockCreator, IPoolService poolService, LevelPresenter levelPresenter)
        {
            _boardView = new BoardView(blockCreator, levelPresenter);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
        }
        
        public void Initialize()
        {
            _boardView.OnBlockCreated += _boardModel.AddCell;
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
            Fill(bottomBlastedLocations, _boardModel.Cells);
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
                if (cell == null || visitedCells.Contains(cell) || cell.CellType == BlockType.Obstacle)
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

        private void Collapse(IEnumerable<BoardLocation> bottomBlastedLocations, Cell[,] cells)
        {
            foreach (var location in bottomBlastedLocations)
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

        private void Fill(IEnumerable<BoardLocation> bottomBlastedLocations, Cell[,] cells)
        {
            var emptyLocations = new List<BoardLocation>();
            var boardHeight = cells.GetLength(1);
            foreach (var location in bottomBlastedLocations)
            {
                for (var i = location.y; i < boardHeight; i++)
                {
                    var cell = cells[location.x, i];
                    if (cell == null)
                    {
                        var emptyLocation = new BoardLocation(location.x, i);
                        emptyLocations.Add(emptyLocation);
                    }
                    else if(cell.CellType == BlockType.Obstacle)
                        emptyLocations.Clear();
                }
            }

            _boardView.Fill(emptyLocations, boardHeight);
        }

        private void Shuffle()
        {
        }

        public void Dispose()
        {
            _boardView.OnBlockCreated -= _boardModel.AddCell;
        }
    }
}