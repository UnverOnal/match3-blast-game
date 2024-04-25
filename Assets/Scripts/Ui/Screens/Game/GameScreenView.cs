using GameState;
using Ui.Animation.Transition;

namespace UI.Screens.Game
{
    public class GameScreenView : ScreenView
    {
        private readonly GameScreenResources _resources;
        private readonly GameStatePresenter _statePresenter;

        public GameScreenView(GameScreenResources screenResources, GameStatePresenter statePresenter) : base(screenResources)
        {
            _resources = screenResources;
            _statePresenter = statePresenter;
            CreateTransitions(UiTransitionType.Slide, screenResources.slideData);
        }

        public void OnHomeButtonClicked()
        {
            _statePresenter.UpdateGameState(GameManagement.GameState.GameState.Home);
        }
    }
}