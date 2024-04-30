using System;
using UnityEngine;

namespace MoveManagement.TimerSystem
{
    public class TimerPresenter
    {
        public event Action OnComplete;

        public bool IsRunning { get; private set; }

        private readonly TimerModel _timerModel;
        private readonly TimerView _timerView;
        
        public TimerPresenter(MoveResources moveResources)
        {
            _timerModel = new TimerModel();
            _timerView = new TimerView(moveResources.timerText);

            _timerModel.OnTimerChange += UpdateView;
            OnComplete += Reset;
        }

        public void SetDuration(float duration)
        {
            _timerModel.SetDuration(duration);
        }

        public void StartTimer()
        {
            _timerView.StartTimer();
            IsRunning = true;
        }

        public void StopTimer()
        {
            IsRunning = false;
            _timerView.StopTimer();
        }

        public void Update()
        {
            if (!IsRunning)
                return;

            _timerModel.UpdateRemainingTime(Mathf.Max(0f, _timerModel.RemainingTime - Time.deltaTime));

            if (_timerModel.RemainingTime <= 0f)
                OnComplete?.Invoke();
        }

        private void UpdateView()
        {
            _timerView.UpdateTimerText(Mathf.CeilToInt(_timerModel.RemainingTime));
        }

        private void Reset()
        {
            StopTimer();
            _timerModel.ResetTimer();
        }
    }
}