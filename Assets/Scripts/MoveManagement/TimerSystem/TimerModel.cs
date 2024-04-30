using System;
using System.Globalization;
using UnityEngine;

namespace MoveManagement.TimerSystem
{
    public class TimerModel
    {
        public event Action OnTimerChange;

        private const string RemainingKey = "RemaningTime";
        private const string QuitTimeKey = "QuitTime";

        public float RemainingTime { get; set; }
        private readonly float _totalTime;
        
        public DateTime QuitTime { get; private set; }
        
        public TimerModel(float totalTime)
        {
            _totalTime = totalTime;
            
            GetData();
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

        private void GetData()
        {
            RemainingTime = PlayerPrefs.GetFloat(RemainingKey, _totalTime);
            
            var convertedQuitTime =
                PlayerPrefs.GetString(QuitTimeKey, DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
            QuitTime = DateTime.Parse(convertedQuitTime);
        }
    }
}