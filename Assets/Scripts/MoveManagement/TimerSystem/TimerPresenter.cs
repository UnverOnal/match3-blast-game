using System;
using UnityEngine;

namespace MoveManagement.TimerSystem
{
    public class TimerPresenter
    {
        public event Action OnTimerComplete;

        public bool IsRunning { get; private set; }

        private readonly TimerModel _timerModel;
        private readonly TimerView _timerView;

        private readonly int _duration;

        public TimerPresenter(MoveResources moveResources, int duration)
        {
            _duration = duration;
            _timerModel = new TimerModel(_duration);
            _timerView = new TimerView(moveResources.timerText);

            _timerModel.OnTimerChange += UpdateView;
            OnTimerComplete += ResetTimer;
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
                OnTimerComplete?.Invoke();
        }

        // public void CalculateRemainingTime()
        // {
        //     if (Math.Abs(_timerModel.RemainingTime - _duration) < 0.1f)
        //         return;
        //
        //     var elapsedTime = (float)(DateTime.Now - _timerModel.QuitTime).TotalSeconds;
        //     if (elapsedTime < _timerModel.RemainingTime)
        //     {
        //         _timerModel.RemainingTime -= elapsedTime;
        //         StartTimer();
        //     }
        //     else
        //     {
        //         OnTimerComplete?.Invoke();
        //     }
        // }

        private void UpdateView()
        {
            _timerView.UpdateTimerText(Mathf.CeilToInt(_timerModel.RemainingTime));
        }

        private void ResetTimer()
        {
            StopTimer();
            _timerModel.ResetTimer();
        }
    }
}