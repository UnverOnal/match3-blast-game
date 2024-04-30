using GamePlay.CellManagement;

namespace GamePlay.Mediator
{
    public interface IMediator
    {
        void NotifyBlast(CellGroup selectedGroup, BoardLocation selectedBlockLocation);
    }
}
