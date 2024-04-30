using Game.Scripts.Ui;
using TMPro;

namespace MoveManagement.TimerSystem
{
    public class TimerView
    {
        private readonly TextMeshProUGUI _timerText;

        public TimerView(TextMeshProUGUI timerText)
        {
            _timerText = timerText;
        }

        public void UpdateTimerText(int time)
        {
            _timerText.text = UiUtil.FormatTime(time);
        }
        
        public void StartTimer()
        {
            _timerText.transform.gameObject.SetActive(true);
        }

        public void StopTimer()
        {
            _timerText.transform.gameObject.SetActive(false);
        }
    }
}