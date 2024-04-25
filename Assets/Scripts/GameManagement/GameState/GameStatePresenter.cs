using System;
using GameManagement.GameState;

namespace GameState
{
    public class GameStatePresenter
    {
        public GameManagement.GameState.GameState CurrentState => _gameStateModel.GameState;
        public event Action<GameManagement.GameState.GameState> OnStateUpdate;

        private readonly GameStateModel _gameStateModel;
        
        public GameStatePresenter()
        {
            _gameStateModel = new GameStateModel();
            UpdateGameState(GameManagement.GameState.GameState.Home);
        }

        public void UpdateGameState(GameManagement.GameState.GameState state)
        {
            _gameStateModel.SetGameState(state);
            OnStateUpdate?.Invoke(state);
        }
    }
}
