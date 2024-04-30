using GamePlay.CellManagement;

namespace GoalManagement.Observe
{
    public interface IObserver
    {
        void Update(CellType cellType);
    }
}
