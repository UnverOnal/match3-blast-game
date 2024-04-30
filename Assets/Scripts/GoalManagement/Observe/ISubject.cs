using GamePlay.CellManagement;
using GoalManagement.Goals;

namespace GoalManagement.Observe
{
    public interface ISubject
    {
        void Attach(Goal goal);
        void Detach(Goal goal);
        void Notify(CellType cellType);
    }
}
