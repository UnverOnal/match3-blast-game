using GameManagement;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class GamePlayPresenter : IInitializable
    {
        [Inject] private IInputService _inputService;
        
        public void Initialize()
        {
            _inputService.OnItemPicked += OnCellSelected;
        }

        private void OnCellSelected(GameObject cellGameObject)
        {
            if(cellGameObject.layer != LayerMask.NameToLayer("Cell")) return;
            
        }

        private void OnLevelEnd()
        {
            _inputService.OnItemPicked -= OnCellSelected;
        }
    }
}
