using System;
using GameManagement;
using GameManagement.LifeCycle;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
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
            var levelData = _levelPresenter.GetNextLevelData();
            _boardModel.SetBoardSize(levelData.width, levelData.height);
            _levelCreationView.PlaceBlocks(levelData);
            _levelFitter.AlignCamera(_boardModel.Cells);
            _levelCreationView.SetBoardBackground(_levelFitter.Bounds);
            
            OnLevelCreated?.Invoke();
        }
    }
}