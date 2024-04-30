using System.Collections.Generic;
using GamePlay.CellManagement;
using GoalManagement.Goals;
using GoalManagement.Observe;
using Level.LevelCounter;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GoalManagement
{
    public class GoalPresenter : ISubject
    {
        private readonly ObjectPool<Goal> _goalPool;

        private readonly LevelPresenter _levelPresenter;

        private readonly List<Goal> _goals;
        
        [Inject]
        public GoalPresenter(LevelPresenter levelPresenter, IPoolService poolService)
        {
            _levelPresenter = levelPresenter;
            
            _goalPool = poolService.GetPoolFactory().CreatePool(()=> (Goal)new CounterGoal());

            _goals = new List<Goal>();
        }

        public void CreateGoals()
        {
            var levelData = _levelPresenter.GetCurrentLevelData();
            var goalDatas = levelData.counterGoalDatas;
            
            foreach (var goalData in goalDatas)
            {
                var goal = _goalPool.Get();
                goal.SetData(goalData);
                Attach(goal);
            }
        }

        public void Attach(Goal goal) => _goals.Add(goal);

        public void Detach(Goal goal)
        {
            _goals.Remove(goal);
            goal.Reset();
            _goalPool.Return(goal);
        }

        public void Notify(CellType cellType)
        {
            for (int i = 0; i < _goals.Count; i++)
            {
                var goal = _goals[i];
                goal.Update(cellType);
                if(goal.IsCompleted())
                    Detach(goal);
            }
            
            if(_goals.Count < 1)
                Debug.Log("Complete");
        }
    }
}
