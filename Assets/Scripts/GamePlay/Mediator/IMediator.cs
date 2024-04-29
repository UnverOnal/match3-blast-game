using GamePlay.CellManagement;

namespace GamePlay.Mediator
{
    public interface IMediator
    {
        void Notify(CellGroup selectedGroup, BoardLocation selectedBlockLocation);
    }
}
