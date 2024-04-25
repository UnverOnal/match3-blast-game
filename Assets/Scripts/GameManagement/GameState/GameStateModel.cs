namespace GameManagement.GameState
{
    public enum GameState
    {
        Home,
        Game,
        LevelEnd
    }
    
    public class GameStateModel
    {
        public GameState GameState { get; private set; }

        public void SetGameState(GameState state)
        {
            GameState = state;
        }
    }
}
