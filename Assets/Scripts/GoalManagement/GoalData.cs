using System;
using GamePlay.CellManagement;


namespace GoalManagement
{
    [Serializable]
    public class GoalData
    {
        public string description;
    }
    
    [Serializable]
    public class CounterData : GoalData
    {
        public CellType cellType;
        public int target;
    }
}
