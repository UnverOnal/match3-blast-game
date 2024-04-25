using System.Collections.Generic;
using VContainer;

namespace GameManagement
{
    public class GameSceneManager : VContainer.Unity.IInitializable
    {
        private readonly IEnumerable<IInitializable> _initializables;

        [Inject]
        public GameSceneManager(IEnumerable<IInitializable> initializables)
        {
            _initializables = initializables;
        }

        public void Initialize()
        {
            foreach (var initializable in _initializables)
                initializable.Initialize();
        }
    }
}