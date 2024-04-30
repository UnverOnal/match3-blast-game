using GamePlay.CellManagement;

namespace GoalManagement.Goals
{
    public class CounterGoal : Goal
    {
        private CellType _cellType;
        private int _target;
        private int _currentCount;
        
        public override void SetData(GoalData goalData)
        {
            base.SetData(goalData);
            var counterData = (CounterData)goalData;
            _cellType = counterData.cellType;
            _target = counterData.target;
        }

        public override void Reset()
        {
            base.Reset();
            _cellType = CellType.None;
        }

        public override bool IsCompleted() => _currentCount >= _target;

        public override void Update(CellType cellType)
        {
            if(cellType != _cellType)
                return;

            _currentCount++;
        }
    }
}
