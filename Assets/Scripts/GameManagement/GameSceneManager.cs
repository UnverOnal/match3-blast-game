using UI;
using VContainer;
using VContainer.Unity;

namespace GameManagement
{
    public class GameSceneManager : IInitializable
    {
        private readonly UiManager _uiManager;

        [Inject]
        public GameSceneManager(UiManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Initialize()
        {
            _uiManager.Initialize();
        }
    }
}
