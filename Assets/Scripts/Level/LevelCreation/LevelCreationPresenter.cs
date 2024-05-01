using System;
using GameManagement;
using GameManagement.LifeCycle;
using GamePlay;
using GamePlay.Board;
using GamePlay.CellManagement;
using Level.LevelCounter;
using Services.PoolingService;
using VContainer;

namespace Level.LevelCreation
{
    public class LevelCreationPresenter : IInitialize
    {
        public event Action OnLevelCreated;
        
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;
        [Inject] private IPoolService _poolService;
        [Inject] private CellCreator _cellCreator;

        private readonly LevelCreationView _levelCreationView;
        private readonly LevelFitter _levelFitter;
        
        [Inject]
        public LevelCreationPresenter(BoardCreationData creationData, GameSettings gameSettings, BoardResources boardResources, CellPrefabCreator cellPrefabCreator)
        {
            _levelCreationView = new LevelCreationView(creationData, cellPrefabCreator, boardResources);

            _levelFitter = new LevelFitter(gameSettings);
        }
        
        public void Initialize()
        {
            _levelCreationView.OnPlaceBlock += _cellCreator.AddCell;
        }

        public void Create()
        {
            ResetBoard(_boardModel.Cells);

            var levelData = _levelPresenter.GetNextLevelData();
            _boardModel.SetBoardSize(levelData.width, levelData.height);
            _levelCreationView.CreateBoard(levelData);
            _levelFitter.AlignCamera(_boardModel.Cells);
            _levelCreationView.SetBoardBackground(_levelFitter.Bounds);
            
            OnLevelCreated?.Invoke();
        }

        private void ResetBoard(Cell[,] board)
        {
            if(board == null)
                return;
            
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            
            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                var cell = board[i, j];
                
                if(cell == null) continue;

                _levelCreationView.ResetCell(cell);
                _cellCreator.RemoveCell(cell);
                cell.Reset();
            }
        }
    }
}