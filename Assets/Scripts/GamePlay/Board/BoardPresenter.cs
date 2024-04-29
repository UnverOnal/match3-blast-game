using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameManagement;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using GamePlay.PrefabCreation;
using Level.LevelCounter;
using Services.InputService;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BoardPresenter : Colleague, IInitializable, IDisposable
    {
        [Inject] private BoardModel _boardModel;
        [Inject] private CellCreator _cellCreator;
        [Inject] private IInputService _inputService;
        private readonly BoardView _boardView;
        
        private readonly BoardShuffler _boardShuffler;
        private readonly BoardGrouper _boardGrouper;

        private readonly ObjectPool<CellGroup> _groupPool;
        
        [Inject]
        public BoardPresenter(CellPrefabCreator cellPrefabCreator, IPoolService poolService, LevelPresenter levelPresenter, GameSettings gameSettings)
        {
            _boardView = new BoardView(cellPrefabCreator, levelPresenter, gameSettings.blockMovementData);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
            _boardGrouper = new BoardGrouper(_groupPool);
            _boardShuffler = new BoardShuffler();
        }
        
        public void Initialize()
        {
            _boardView.OnFillBlock += _cellCreator.AddCell;
        }

        public async void OnBlockSelected(GameObject selectedBlock)
        {
            moveMediator.Notify();
            var selectedGroup = _boardModel.GetGroup(selectedBlock);
            if (selectedGroup == null)
            {
                _boardView.Shake(selectedBlock.transform, 0.1f, 30f);
                return;
            }
            
            await Blast(selectedGroup, selectedBlock);

            //Gets bottom ones for being able to start checking from them to top.
            var bottomBlastedLocations = selectedGroup.bottomLocations.Select(pair => pair.Value).ToList();
            ResetGroup(selectedGroup);
            Collapse(bottomBlastedLocations, _boardModel.Cells);
            await Fill(bottomBlastedLocations, _boardModel.Cells);
            
            GroupCells();
            if(_boardModel.cellGroups.Count < 1)
                Shuffle();
        }

        public void GroupCells() => _boardGrouper.GroupCells(_boardModel);

        private void ResetGroup(CellGroup group)
        {
            foreach (var cell in group.blocks)
                _cellCreator.RemoveCell(cell);
            
            foreach (var explodeable in group.explodeableObstacles)
                _cellCreator.RemoveCell(explodeable as Cell);
            
            _boardModel.RemoveCellGroup(group);
            group.Reset();
            _groupPool.Return(group);
        }

        private async UniTask Blast(CellGroup selectedGroup, GameObject selectedBlock)
        {
            selectedGroup.DamageNeighbours(_boardModel.Cells);
            _boardView.ExplodeDamageables(selectedGroup);
            await _boardView.Blast(selectedGroup, selectedBlock);
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
                    if (cell.GetType() == typeof(Obstacle))
                    {
                        yLocation = cell.Location.y + 1;
                        continue;
                    }

                    _boardModel.UpdateCellLocation(cell, new BoardLocation(location.x, yLocation++));
                    _boardView.Collapse(cell);
                }
            }
        }

        private async UniTask Fill(IEnumerable<BoardLocation> bottomBlastedLocations, Cell[,] cells)
        {
            var allEmptyLocations = new List<List<BoardLocation>>();
            var boardHeight = cells.GetLength(1);
            foreach (var location in bottomBlastedLocations)
            {
                var columnEmptyLocations = new List<BoardLocation>();
                for (var i = location.y; i < boardHeight; i++)
                {
                    var cell = cells[location.x, i];
                    if (cell == null)
                    {
                        var emptyLocation = new BoardLocation(location.x, i);
                        columnEmptyLocations.Add(emptyLocation);
                    }
                    else if(cell.GetType() == typeof(Obstacle))
                        columnEmptyLocations.Clear();
                }
                allEmptyLocations.Add(columnEmptyLocations);
            }

            //Fill all at once because of taking necessity of leaving empty under the obstacle account.
            await _boardView.Fill(allEmptyLocations, boardHeight);
        }

        private async void Shuffle()
        {
            _inputService.IgnoreInput(true);
            while (_boardModel.cellGroups.Count < 1)
            {
                _boardShuffler.Shuffle(_boardModel.Cells);
                GroupCells();
            }
            
            await _boardView.Shuffle(_boardModel.Cells);
            _inputService.IgnoreInput(false);
        }

        public void Dispose()
        {
            _boardView.OnFillBlock -= _cellCreator.AddCell;
        }
    }
}