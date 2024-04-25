using System;
using GameState;

namespace UI.Screens
{
    public abstract class ScreenPresenter : IScreenPresenter, IDisposable
    {
        private readonly GameStatePresenter _gameStatePresenter;

        protected ScreenPresenter(GameStatePresenter gameStatePresenter)
        {
            _gameStatePresenter = gameStatePresenter;
        }
        
        public virtual void Initialize()
        {
            SetStateAction();
        }

        protected abstract void OnStateUpdate(GameManagement.GameState.GameState gameState);
        
        private void SetStateAction()
        {
            _gameStatePresenter.OnStateUpdate += OnStateUpdate;
        }

        public virtual void Dispose()
        {
            _gameStatePresenter.OnStateUpdate += OnStateUpdate;
        }
    }
}
