using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameManagement;
using GameManagement.LifeCycle;
using GamePlay.Board.Steps;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;
using GamePlay.Mediator;
using GoalManagement;
using Services.InputService;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BoardPresenter : Colleague, IInitialize, IDisposable
    {
        [Inject] private BoardModel _boardModel;
        [Inject] private CellCreator _cellCreator;
        [Inject] private IInputService _inputService;
        [Inject] private BoardFillPresenter _fillPresenter;
        [Inject] private GoalPresenter _goalPresenter;

        private readonly BoardView _boardView;

        private readonly BoardShuffler _boardShuffler;
        private readonly BoardGrouper _boardGrouper;

        private readonly ObjectPool<CellGroup> _groupPool;

        private readonly int[] _previousGroupBounds;

        [Inject]
        public BoardPresenter(CellPrefabCreator cellPrefabCreator, IPoolService poolService, GameSettings gameSettings)
        {
            _boardView = new BoardView(cellPrefabCreator, gameSettings.blockMovementData);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
            _boardGrouper = new BoardGrouper(_groupPool);
            _boardShuffler = new BoardShuffler();
            _previousGroupBounds = new[]{int.MinValue, int.MinValue };
        }

        public void Initialize()
        {
            _boardModel.OnCellRemove += _goalPresenter.Notify;
        }

        public async void OnBlockSelected(GameObject selectedBlock)
        {
            var selectedBlockLocation = _boardModel.GetCell(selectedBlock).Location;
            var selectedGroup = _boardModel.GetGroup(selectedBlock);
            
            if (IgnoreMove(selectedGroup)) return;

            if (selectedGroup == null)
            {
                _boardView.Shake(selectedBlock.transform, 0.1f, 30f);
                return;
            }

            await Blast(selectedGroup, selectedBlock);
            matchMediator.NotifyBlast(selectedGroup, selectedBlockLocation);

            //Gets bottom ones for being able to start checking from them to top.
            var bottomBlastedLocations = selectedGroup.bottomLocations.Select(pair => pair.Value).ToList();
            await Fill(bottomBlastedLocations, _boardModel.Cells);

            GroupCells();
            if (_boardModel.cellGroups.Count < 1)
                Shuffle();

            ResetGroup(selectedGroup);
        }

        public void GroupCells() => _boardGrouper.GroupCells(_boardModel);

        private async UniTask Blast(CellGroup selectedGroup, GameObject selectedBlock)
        {
            selectedGroup.DamageNeighbours(_boardModel.Cells);
            _boardView.ExplodeDamageables(selectedGroup);
            await _boardView.Blast(selectedGroup, selectedBlock);
            RemoveGroupCells(selectedGroup);
        }

        private void RemoveGroupCells(CellGroup group)
        {
            foreach (var cell in group.blocks)
            {
                _boardModel.RemoveCell(cell);
                _cellCreator.ReturnCell(cell);
            }
            
            foreach (var explodable in group.explodeableObstacles)
            {
                var cell = (Cell)explodable;
                _boardModel.RemoveCell(cell);
                _cellCreator.ReturnCell(cell);
                cell.Reset();
            }
        }

        private void ResetGroup(CellGroup group)
        {
            _boardModel.RemoveCellGroup(group);
            group.Reset();
            _groupPool.Return(group);
            SetPreviousBounds(int.MaxValue, int.MinValue);
        }

        private async UniTask Fill(IEnumerable<BoardLocation> bottomBlastedLocations, Cell[,] cells)
        {
            var tasks = new List<UniTask>();
            foreach (var location in bottomBlastedLocations)
            {
                _fillPresenter.CollapseColumn(location, cells);
                var task = _fillPresenter.FillColumn(location, cells);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
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

        private bool IgnoreMove(CellGroup selectedGroup)
        {
            if (selectedGroup == null)
                return false;
            
            var ignore = selectedGroup.HasIntersectionWith(_previousGroupBounds);

            var xBounds = selectedGroup.xBounds;
            SetPreviousBounds(xBounds[0], xBounds[1]);
            
            return ignore;
        }

        private void SetPreviousBounds(int start, int end)
        {
            _previousGroupBounds[0] = start;
            _previousGroupBounds[1] = end;
        }

        public void Dispose() => _boardModel.OnCellRemove -= _goalPresenter.Notify;
    }
}