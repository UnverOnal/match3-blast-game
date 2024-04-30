namespace MoveManagement
{
    public class MoveModel
    {
        public int MoveCount { get; private set; }
        public void SetMoveCount(int moveCount) => MoveCount = moveCount;

        public void UpdateMoveCount() => MoveCount--;

        public void Reset() => MoveCount = 0;
    }
}
