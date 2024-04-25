using Board.BoardCreation;
using GameState;
using VContainer;

namespace UI.Screens.Home
{
    public class HomeScreenPresenter : ScreenPresenter
    {
        private readonly HomeScreenResources _resources;
        private readonly HomeScreenView _screenView;

        [Inject] private BoardCreationPresenter _boardCreationPresenter;
        
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

        public void PlayButton()
        {
            _resources.playButton.onClick.AddListener(_screenView.OnPlayButtonClicked);
            _resources.playButton.onClick.AddListener(_boardCreationPresenter.Create);
        }
    }
}