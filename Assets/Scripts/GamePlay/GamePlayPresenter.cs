using System;
using GameManagement.LifeCycle;
using GamePlay.Mediator;
using GameState;
using GoalManagement;
using Level.LevelCreation;
using MoveManagement;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class GamePlayPresenter : Colleague, IInitialize, IDisposable
    {
        [Inject] private IInputService _inputService;
        [Inject] private LevelCreationPresenter _levelCreationPresenter;
        [Inject] private GoalPresenter _goalPresenter;
        [Inject] private GameStatePresenter _gameStatePresenter;
        [Inject] private MovePresenter _movePresenter;

        public void Initialize()
        {
            _inputService.OnItemPicked += OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated += OnLevelStart;
            _goalPresenter.OnGoalsDone += OnLevelEnd;
        }

        private void OnBlockSelected(GameObject cellGameObject)
        {
            if(cellGameObject.layer == LayerMask.NameToLayer("Default")) return;
            
            matchMediator.NotifyOnInput(cellGameObject);
        }

        private void OnLevelStart()
        {
            _inputService.IgnoreInput(false);
            matchMediator.NotifyLevelStart();
            _goalPresenter.CreateGoals();
        }

        public void OnLevelEnd()
        {
            _movePresenter.Reset();
            _gameStatePresenter.UpdateGameState(GameManagement.GameState.GameState.Home);
            _inputService.IgnoreInput(true);
        }

        public void Dispose()
        {
            _inputService.OnItemPicked -= OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated -= _goalPresenter.CreateGoals;
            _goalPresenter.OnGoalsDone -= OnLevelEnd;
        }
    }
}
