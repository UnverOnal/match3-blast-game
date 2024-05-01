using System;
using System.Collections.Generic;
using GamePlay.CellManagement;
using GoalManagement.Observe;
using Level.LevelCounter;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GoalManagement
{
    public class GoalPresenter : ISubject
    {
        public event Action OnGoalsDone;
        
        private readonly GoalView _goalView;

        private readonly ObjectPool<Goal> _goalPool;
        private readonly LevelPresenter _levelPresenter;

        private readonly List<Goal> _goals;

        [Inject]
        public GoalPresenter(LevelPresenter levelPresenter, IPoolService poolService,
            BoardCreationData boardCreationData, GoalResources goalResources)
        {
            _levelPresenter = levelPresenter;

            _goalPool = poolService.GetPoolFactory().CreatePool(() => new Goal());

            _goals = new List<Goal>();

            _goalView = new GoalView(boardCreationData, goalResources, poolService);
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
                _goalView.CreateMove(goal);
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
            if(_goals.Count < 1)
                return;
            
            for (var i = 0; i < _goals.Count; i++)
            {
                var goal = _goals[i];
                if(cellType != goal.cellType) continue;
                
                goal.Update(cellType);
                _goalView.UpdateMove(goal);
                if (goal.IsCompleted())
                    Detach(goal);
            }

            if (_goals.Count > 0) return;
            
            _goalView.Reset();
            OnGoalsDone?.Invoke();
        }
    }
}