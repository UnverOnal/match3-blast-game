using GameState;
using UI.Screens;
using UI.Screens.Game.LevelEnd;
using VContainer;

namespace Ui.Screens.LevelEnd 
{
    public class LevelEndScreenPresenter : ScreenPresenter
    {
        private readonly LevelEndScreenResources _screenResources;
        private readonly GameStatePresenter _statePresenter;
        private readonly LevelEndScreenView _screenView;
        private readonly LevelEndScreenModel _levelEndScreenModel;

        [Inject]
        public LevelEndScreenPresenter(LevelEndScreenResources screenResources, GameStatePresenter statePresenter) : base(statePresenter)
        {
            _screenResources = screenResources;
            _statePresenter = statePresenter;
            _screenView = new LevelEndScreenView(screenResources);
            _levelEndScreenModel = new LevelEndScreenModel();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnStateUpdate(GameManagement.GameState.GameState gameState)
        {
            if (gameState == GameManagement.GameState.GameState.LevelEnd)
                _screenView.Enable();
            else if(_screenView.IsActive)
                _screenView.Disable();
        }
    }
}