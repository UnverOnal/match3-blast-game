using GameManagement;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using PowerUpManagement;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class MoveMediator : IMediator, IInitializable
    {
        private readonly BoardPresenter _boardPresenter;
        private readonly PowerUpPresenter _powerUpPresenter;
        private readonly GamePlayPresenter _gamePlayPresenter;

        [Inject]
        public MoveMediator(BoardPresenter boardPresenter, PowerUpPresenter powerUpPresenter, GamePlayPresenter gamePlayPresenter)
        {
            _boardPresenter = boardPresenter;
            _powerUpPresenter = powerUpPresenter;
            _gamePlayPresenter = gamePlayPresenter;
        }
        
        public void Initialize()
        {
            SetMediator(_boardPresenter);
            SetMediator(_powerUpPresenter);
            SetMediator(_gamePlayPresenter);
        }

        private void SetMediator(Colleague colleague) => colleague.SetMediator(this);

        public void NotifyBlast(CellGroup selectedGroup, BoardLocation selectedBlockLocation)
        {
            _powerUpPresenter.CreatePowerUp(selectedGroup, selectedBlockLocation);
        }
        
        public async void NotifyOnInput(GameObject selectedGameObject)
        {
            var layer = selectedGameObject.layer;
            
            if(layer == LayerMask.NameToLayer("Cell"))
                _boardPresenter.OnBlockSelected(selectedGameObject);
            else if (layer == LayerMask.NameToLayer("PowerUp"))
            {
                await _powerUpPresenter.Explode(selectedGameObject);
                _boardPresenter.GroupCells();
            }
        }
    }
}
