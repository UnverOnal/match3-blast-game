using GamePlay.CellManagement;
using GoalManagement.Observe;

namespace GoalManagement
{
    public class Goal : IObserver
    {
        public CellType cellType;
        public int CurrentCount { get; private set; }
        public int Target { get; private set; }

        protected string description;

        public void SetData(GoalData goalData)
        {
            var counterData = (CounterData)goalData;
            cellType = counterData.cellType;
            Target = counterData.target;
            description = goalData.description;
        }

        public void Reset()
        {
            description = string.Empty;
            cellType = CellType.None;
        }

        public bool IsCompleted()
        {
            return CurrentCount >= Target;
        }

        public void Update(CellType type) => CurrentCount++;
    }
}