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
    public class MatchMediator : IMediator, IInitialize, IDisposable
    {
        [Inject]private readonly BoardPresenter _boardPresenter;
        [Inject]private readonly PowerUpPresenter _powerUpPresenter;
        [Inject]private readonly GamePlayPresenter _gamePlayPresenter;
        [Inject]private readonly MovePresenter _movePresenter;

        public void Initialize()
        {
            SetMediator(_boardPresenter);
            SetMediator(_powerUpPresenter);
            SetMediator(_gamePlayPresenter);
            SetMediator(_movePresenter);

            _movePresenter.OnMovesDone += _gamePlayPresenter.OnLevelEnd;
        }

        private void SetMediator(Colleague colleague) => colleague.SetMediator(this);

        public void NotifyLevelStart()
        {
            _boardPresenter.GroupCells();
            _powerUpPresenter.SetCanIgnoreInput(true);
        }        
        
        public void NotifyLevelEnd()
        {
            _movePresenter.Reset();
            _powerUpPresenter.SetCanIgnoreInput(false);
        }

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
