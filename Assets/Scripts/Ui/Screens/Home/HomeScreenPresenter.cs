using GameState;
using Level.LevelCreation;
using VContainer;

namespace UI.Screens.Home
{
    public class HomeScreenPresenter : ScreenPresenter
    {
        private readonly HomeScreenResources _resources;
        private readonly HomeScreenView _screenView;

        [Inject] private LevelCreationPresenter _levelCreationPresenter;
        
        [Inject]
        public HomeScreenPresenter(HomeScreenResources resources, GameStatePresenter statePresenter) : base(statePresenter)
        {
            _resources = resources;
            _screenView = new HomeScreenView(resources, statePresenter);
        }

        public override void Initialize()
        {
            base.Initialize();
            _screenView.Enable();
            PlayButton();
        }

        protected override void OnStateUpdate(GameManagement.GameState.GameState gameState)
        {
            if(gameState == GameManagement.GameState.GameState.Home)
                _screenView.Enable();
            else if(_screenView.IsActive)
                _screenView.Disable();
        }

        private void PlayButton()
        {
            _resources.playButton.onClick.AddListener(_screenView.OnPlayButtonClicked);
            _resources.playButton.onClick.AddListener(_levelCreationPresenter.Create);
        }
    }
}