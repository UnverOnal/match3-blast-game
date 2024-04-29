using GamePlay.Board;
using GamePlay.Mediator;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class MoveMediator : IMediator
    {
        private BoardPresenter _boardPresenter;

        [Inject]
        public MoveMediator(BoardPresenter boardPresenter)
        {
            _boardPresenter = boardPresenter;
            SetMediator(boardPresenter);
        }

        public void Notify()
        {
            Debug.Log("Notified");
        }
        
        private void SetMediator(Colleague colleague) => colleague.SetMediator(this);
    }
}
