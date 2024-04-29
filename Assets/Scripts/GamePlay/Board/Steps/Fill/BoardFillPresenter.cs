using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameManagement;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
using Level.LevelCounter;
using VContainer;

namespace GamePlay.Board.Steps.Fill
{
    public class BoardFillPresenter : IInitializable, IDisposable
    {
        [Inject] private BoardModel _boardModel;
        [Inject] private CellCreator _cellCreator;

        private readonly BoardFillView _boardFillView;

        [Inject]
        public BoardFillPresenter(CellPrefabCreator cellPrefabCreator, GameSettings gameSettings, LevelPresenter levelPresenter)
        {
            _boardFillView = new BoardFillView(gameSettings.blockMovementData, cellPrefabCreator, levelPresenter);
        }
        
        public void Initialize()
        {
            _boardFillView.OnFillBlock += _cellCreator.AddCell;
        }

        public async UniTask FillColumn(BoardLocation bottomLocation, Cell[,] cells)
        {
            var boardHeight = cells.GetLength(1);
            var columnEmptyLocations = new List<BoardLocation>();
            for (var i = bottomLocation.y; i < boardHeight; i++)
            {
                var cell = cells[bottomLocation.x, i];
                if (cell == null)
                {
                    var emptyLocation = new BoardLocation(bottomLocation.x, i);
                    columnEmptyLocations.Add(emptyLocation);
                }
                else if(cell.GetType() == typeof(Obstacle))
                    columnEmptyLocations.Clear();
            }
            await _boardFillView.FillColumn(columnEmptyLocations, boardHeight);
        }
        
        public void CollapseColumn(BoardLocation bottomLocation, Cell[,] cells)
        {
            var cellsToCollapse = new List<Cell>();
            var yLocation = bottomLocation.y;
            for (var j = bottomLocation.y; j < cells.GetLength(1); j++)
            {
                var cell = cells[bottomLocation.x, j];
                if (cell == null)
                    continue;
                if (cell.GetType() == typeof(Obstacle))
                {
                    yLocation = cell.Location.y + 1;
                    continue;
                }

                _boardModel.UpdateCellLocation(cell, new BoardLocation(bottomLocation.x, yLocation++));
                cellsToCollapse.Add(cell);
            }
            _boardFillView.CollapseColumn(cellsToCollapse);
        }

        public void Dispose()
        {
            _boardFillView.OnFillBlock -= _cellCreator.AddCell;
        }
    }
}
