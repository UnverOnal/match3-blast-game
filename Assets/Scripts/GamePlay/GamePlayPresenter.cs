using Board;
using GameManagement;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class GamePlayPresenter : IInitializable
    {
        [Inject] private IInputService _inputService;
        [Inject] private BoardPresenter _boardPresenter;
        
        public void Initialize()
        {
            _inputService.OnItemPicked += OnBlockSelected;
        }

        private void OnBlockSelected(GameObject cellGameObject)
        {
            if(cellGameObject.layer != LayerMask.NameToLayer("Cell")) return;
            
            _boardPresenter.OnBlockSelected(cellGameObject);
        }

        private void OnLevelEnd()
        {
            _inputService.OnItemPicked -= OnBlockSelected;
        }
    }
}
