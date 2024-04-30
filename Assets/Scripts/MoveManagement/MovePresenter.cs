using System;
using GameManagement.LifeCycle;
using GamePlay.Mediator;
using Level.LevelCounter;
using Level.LevelCreation;
using MoveManagement.TimerSystem;
using VContainer;

namespace MoveManagement
{
    public class MovePresenter : Colleague, IInitialize, IUpdate, IDisposable
    {
        public event Action OnMovesDone;

        private readonly LevelCreationPresenter _levelCreationPresenter;
        [Inject] private LevelPresenter _levelPresenter;

        private MoveModel _moveModel;
        private MoveView _moveView;
        private readonly TimerPresenter _timerPresenter;

        [Inject]
        public MovePresenter(MoveResources moveResources, LevelCreationPresenter levelCreationPresenter)
        {
            _levelCreationPresenter = levelCreationPresenter;
            _moveModel = new MoveModel();
            _moveView = new MoveView(moveResources.moveCountText);
            _timerPresenter = new TimerPresenter(moveResources);
        }

        public void Initialize()
        {
            _levelCreationPresenter.OnLevelCreated += SetTimer;
            _levelCreationPresenter.OnLevelCreated += SetMoveCount;
            _timerPresenter.OnComplete += OnDone;
        }

        public void UpdateMoveCount()
        {
            _moveModel.UpdateMoveCount();
            _moveView.SetMoveCount(_moveModel.MoveCount);

            if (_moveModel.MoveCount <= 0)
                OnDone();
        }

        public void Update()
        {
            _timerPresenter.Update();
        }

        public void Dispose()
        {
            _levelCreationPresenter.OnLevelCreated -= SetTimer;
            _levelCreationPresenter.OnLevelCreated -= SetMoveCount;
            _timerPresenter.OnComplete += OnDone;
        }
        
        private void SetTimer()
        {
            var levelData = _levelPresenter.GetCurrentLevelData();
            _timerPresenter.SetDuration(levelData.moveData.duration);
            _timerPresenter.StartTimer();
        }

        private void SetMoveCount()
        {
            var levelData = _levelPresenter.GetCurrentLevelData();
            var moveData = levelData.moveData;
            _moveModel.SetMoveCount(moveData.moveAmount);
            _moveView.Start();
            _moveView.SetMoveCount(moveData.moveAmount);
        }

        private void OnDone()
        {
            OnMovesDone?.Invoke();
            Reset();
        }

        private void Reset()
        {
            _moveModel.Reset();
            _moveView.Stop();

            _timerPresenter.Reset();
        }
    }
}