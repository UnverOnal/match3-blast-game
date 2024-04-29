using GamePlay.Board;
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

        public void Notify()
        {
            Debug.Log("Notified");
        }
        
        private void SetMediator(Colleague colleague) => colleague.SetMediator(this);
    }
}
