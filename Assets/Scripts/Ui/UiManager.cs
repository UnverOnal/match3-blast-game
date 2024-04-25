using System.Collections.Generic;
using GameManagement;
using UI.Screens;
using VContainer;

namespace UI
{
    public class UiManager : IInitializable
    {
        private readonly IEnumerable<IScreenPresenter> _screenPresenters;

        [Inject]
        public UiManager(IEnumerable<IScreenPresenter> screenPresenters)
        {
            _screenPresenters = screenPresenters;
        }

        public void Initialize()
        {
            foreach (var screenPresenter in _screenPresenters)
                screenPresenter.Initialize();
        }
    }
}