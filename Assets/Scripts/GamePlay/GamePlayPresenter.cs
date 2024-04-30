using GameManagement.LifeCycle;
using GamePlay.Board;
using GamePlay.Mediator;
using Level.LevelCreation;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class GamePlayPresenter : Colleague, IInitialize
    {
        [Inject] private IInputService _inputService;
        [Inject] private BoardPresenter _boardPresenter;
        [Inject] private LevelCreationPresenter _levelCreationPresenter;
        
        public void Initialize()
        {
            _inputService.OnItemPicked += OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated += _boardPresenter.GroupCells;
        }

        private void OnBlockSelected(GameObject cellGameObject)
        {
            if(cellGameObject.layer == LayerMask.NameToLayer("Default")) return;
            
            moveMediator.NotifyOnInput(cellGameObject);
        }

        public void OnLevelEnd()
        {
            _inputService.OnItemPicked -= OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated -= _boardPresenter.GroupCells;
        }
    }
}
