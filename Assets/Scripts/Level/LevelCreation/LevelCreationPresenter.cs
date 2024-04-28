using System;
using GameManagement;
using GamePlay.Board;
using GamePlay.CellManagement;
using Level.LevelCounter;
using Services.PoolingService;
using VContainer;

namespace Level.LevelCreation
{
    public class LevelCreationPresenter : IInitializable
    {
        public event Action OnLevelCreated;
        
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;
        [Inject] private IPoolService _poolService;
        [Inject] private CellCreator _cellCreator;

        private readonly LevelCreationView _levelCreationView;
        private readonly LevelFitter _levelFitter;

        [Inject]
        public LevelCreationPresenter(BoardCreationData creationData, GameSettings gameSettings, BoardResources boardResources, BlockCreator blockCreator)
        {
            _levelCreationView = new LevelCreationView(creationData, blockCreator, boardResources);

            _levelFitter = new LevelFitter(gameSettings);
        }
        
        public void Initialize()
        {
            _levelCreationView.OnPlaceBlock += _cellCreator.AddCell;
        }

        public void Create()
        {
            var levelData = _levelPresenter.GetNextLevelData();
            _boardModel.SetBoardSize(levelData.boardSize);
            _levelCreationView.PlaceBlocks(levelData);
            _levelFitter.AlignCamera(_boardModel.Cells);
            _levelCreationView.SetBoardBackground(_levelFitter.Bounds);
            
            OnLevelCreated?.Invoke();
        }
    }
}