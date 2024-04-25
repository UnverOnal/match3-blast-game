using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ui.Animation.Transition;
using Ui.Animation.Transition.TransitionData;

namespace UI.Screens
{
    public abstract class ScreenView
    {
        private readonly ScreenResources _screenResources;
        public bool IsActive { get; private set; }

        private readonly List<IUiTransition> _uiTransitions;
        private readonly UiTransitionFactory _factory;

        protected ScreenView(ScreenResources screenResources)
        {
            _screenResources = screenResources;
            _uiTransitions = new List<IUiTransition>();
            _factory = new UiTransitionFactory();
        }

        public void Enable()
        {
            IsActive = true;
            _screenResources.screenCanvasGroup.alpha = 1f;
            _screenResources.screenGameObject.SetActive(true);
            
            for (var i = 0; i < _uiTransitions.Count; i++)
                _uiTransitions[i].Enable();
        }

        public async void Disable()
        {
            IsActive = false;
            
            var tasks = new List<UniTask>();
            for (var i = 0; i < _uiTransitions.Count; i++)
            {
                var task = _uiTransitions[i].Disable();
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
            _screenResources.screenCanvasGroup.alpha = 0f;
            _screenResources.screenGameObject.SetActive(false);
        }

        protected void CreateTransitions(UiTransitionType type, IEnumerable<UiTransitionData> uiTransitionData)
        {
            _uiTransitions.Add(_factory.GetTransition(type, uiTransitionData));
        }
    }
}