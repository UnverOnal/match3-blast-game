using System;
using System.Globalization;
using UnityEngine;

namespace MoveManagement.TimerSystem
{
    public class TimerModel
    {
        public event Action OnTimerChange;

        public float RemainingTime { get; set; }
        private float _totalTime;
        
        public void SetDuration(float duration)
        {
            _totalTime = duration;
            RemainingTime = duration;
        }

        public void UpdateRemainingTime(float time)
        {
            RemainingTime = time;
            OnTimerChange?.Invoke();
        }

        public void ResetTimer()
        {
            RemainingTime = _totalTime;
        }
    }
}