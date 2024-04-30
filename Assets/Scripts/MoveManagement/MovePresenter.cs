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
        public event Action OnTimerComplete;

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
            _moveView = new MoveView();
            _timerPresenter = new TimerPresenter(moveResources);
        }

        public void Initialize()
        {
            _levelCreationPresenter.OnLevelCreated += SetTimer;
            _timerPresenter.OnComplete += OnTimerCompleteInvoker;
        }

        private void SetTimer()
        {
            var levelData = _levelPresenter.GetCurrentLevelData();
            _timerPresenter.SetDuration(levelData.moveData.duration);
            _timerPresenter.StartTimer();
        }

        public void Update()
        {
            _timerPresenter.Update();
        }

        public void Dispose()
        {
            _levelCreationPresenter.OnLevelCreated -= SetTimer;
            _timerPresenter.OnComplete += OnTimerCompleteInvoker;
        }

        private void OnTimerCompleteInvoker()
        {
            OnTimerComplete?.Invoke();
        }
    }
}