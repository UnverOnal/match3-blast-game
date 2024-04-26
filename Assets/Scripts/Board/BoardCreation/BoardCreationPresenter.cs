using GameManagement;
using Level;
using Services.PoolingService;
using VContainer;

namespace Board.BoardCreation
{
    public class BoardCreationPresenter : IInitializable
    {
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;

        private readonly BoardCreationData _creationData;
        private readonly BoardCreationView _boardCreationView;
        private readonly LevelFitter _levelFitter;

        [Inject]
        public BoardCreationPresenter(IPoolService poolService, BoardCreationData creationData, GameSettings gameSettings, BoardResources boardResources)
        {
            _creationData = creationData;

            var blockCreator = new BlockCreator(poolService, creationData);

            _boardCreationView = new BoardCreationView(creationData, blockCreator, boardResources);

            _levelFitter = new LevelFitter(gameSettings);
        }
        
        public void Initialize()
        {
            _boardCreationView.OnBlockCreated += _boardModel.AddCell;
        }

        public void Create()
        {
            var levelData = _levelPresenter.GetNextLevelData();
            _boardModel.SetBoardSize(levelData.boardSize);
            _boardCreationView.PlaceBlocks(levelData);
            _levelFitter.AlignCamera(_boardModel.board);
            _boardCreationView.SetBoardBackground(_levelFitter.Bounds);
        }
    }
}