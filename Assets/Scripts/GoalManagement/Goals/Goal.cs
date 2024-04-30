using GamePlay.CellManagement;
using GoalManagement.Observe;

namespace GoalManagement.Goals
{
    public abstract class Goal : IObserver
    {
        protected string description;

        public virtual void SetData(GoalData goalData)
        {
            description = goalData.description;
        }

        public virtual void Reset()
        {
            description = string.Empty;
        }

        public abstract bool IsCompleted();
        public abstract void Update(CellType cellType);
    }
}
