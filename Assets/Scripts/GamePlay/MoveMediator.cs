using System;
using GameManagement.LifeCycle;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using MoveManagement;
using PowerUpManagement;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class MoveMediator : IMediator, IInitialize, IDisposable
    {
        private readonly BoardPresenter _boardPresenter;
        private readonly PowerUpPresenter _powerUpPresenter;
        private readonly GamePlayPresenter _gamePlayPresenter;
        private readonly MovePresenter _movePresenter;

        [Inject]
        public MoveMediator(BoardPresenter boardPresenter, PowerUpPresenter powerUpPresenter, GamePlayPresenter gamePlayPresenter, MovePresenter movePresenter)
        {
            _boardPresenter = boardPresenter;
            _powerUpPresenter = powerUpPresenter;
            _gamePlayPresenter = gamePlayPresenter;
            _movePresenter = movePresenter;
        }
        
        public void Initialize()
        {
            SetMediator(_boardPresenter);
            SetMediator(_powerUpPresenter);
            SetMediator(_gamePlayPresenter);
            SetMediator(_movePresenter);

            _movePresenter.OnMovesDone += _gamePlayPresenter.OnLevelEnd;
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
            
            _movePresenter.UpdateMoveCount();
        }

        public void Dispose()
        {
            _movePresenter.OnMovesDone -= _gamePlayPresenter.OnLevelEnd;
        }
    }
}
