using GameState;
using Ui.Animation.Transition;

namespace UI.Screens.Home
{
    public class HomeScreenView : ScreenView
    {
        private readonly GameStatePresenter _statePresenter;
        private readonly HomeScreenResources _resources;

        public HomeScreenView(HomeScreenResources screenResources, GameStatePresenter statePresenter) : base(
            screenResources)
        {
            _statePresenter = statePresenter;
            _resources = screenResources;
            CreateTransitions(UiTransitionType.ScaleUp, _resources.scaleUpData);
        }

        public void OnPlayButtonClicked()
        {
            _statePresenter.UpdateGameState(GameManagement.GameState.GameState.Game);
            // Disable();
        }
    }
}