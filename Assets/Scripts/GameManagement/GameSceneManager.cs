using System.Collections.Generic;
using System.Linq;
using GameManagement.LifeCycle;
using VContainer;
using VContainer.Unity;

namespace GameManagement
{
    public class GameSceneManager : IInitializable, ITickable
    {
        private readonly IEnumerable<IInitialize> _initializables;
        private readonly IEnumerable<IUpdate> _updateables;

        private bool _isUpdateablesEmpty;

        [Inject]
        public GameSceneManager(IEnumerable<IInitialize> initializables, IEnumerable<IUpdate> updateables)
        {
            _initializables = initializables;
            _updateables = updateables;

            _isUpdateablesEmpty = !_updateables.Any();
        }

        public void Initialize()
        {
            foreach (var initializable in _initializables)
                initializable.Initialize();
        }

        public void Tick()
        {
            if (_isUpdateablesEmpty) return;
            foreach (var updateable in _updateables)
                updateable.Update();
        }
    }
}