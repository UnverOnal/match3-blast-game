using System;
using GameManagement;
using GamePlay.Board;
using Level.LevelCounter;
using VContainer;

namespace Level.LevelCreation
{
    public class LevelCreationPresenter : IInitializable
    {
        public event Action OnLevelCreated;
        
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;

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
            _levelCreationView.OnPlaceBlock += _boardModel.AddCell;
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