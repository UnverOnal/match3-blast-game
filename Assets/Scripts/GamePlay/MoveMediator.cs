using System.Collections.Generic;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using PowerUpManagement;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class MoveMediator : IMediator
    {
        private readonly BoardPresenter _boardPresenter;
        private readonly PowerUpPresenter _powerUpPresenter;

        [Inject]
        public MoveMediator(BoardPresenter boardPresenter, PowerUpPresenter powerUpPresenter)
        {
            _boardPresenter = boardPresenter;
            SetMediator(boardPresenter);
            
            _powerUpPresenter = powerUpPresenter;
            SetMediator(powerUpPresenter);
        }

        private void SetMediator(Colleague colleague) => colleague.SetMediator(this);

        public void Notify(CellGroup selectedGroup, BoardLocation selectedBlockLocation)
        {
            _powerUpPresenter.CreatePowerUp(selectedGroup, selectedBlockLocation);
        }
    }
}
