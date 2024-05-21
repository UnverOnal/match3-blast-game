using GameManagement;
using GameManagement.LifeCycle;
using GamePlay;
using GamePlay.Board;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;
using GameState;
using GoalManagement;
using Level.LevelCounter;
using Level.LevelCreation;
using MoveManagement;
using PowerUpManagement;
using UI;
using UI.Screens;
using UI.Screens.Game;
using UI.Screens.Home;
using Ui.Screens.LevelEnd;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private HomeScreenResources homeScreenResources;
        [SerializeField] private GameScreenResources gameScreenResources;
        [SerializeField] private LevelEndScreenResources levelEndScreenResources;

        [SerializeField] private LevelContainer levelContainer;
        [SerializeField] private BoardCreationData boardCreationData;
        
        [SerializeField] private BoardResources boardResources;
        [SerializeField] private MoveResources moveResources;
        [SerializeField] private GoalResources goalResources;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameSceneManager>();
            builder.Register<IInitialize, UiManager>(Lifetime.Singleton).AsSelf();
            builder.Register<GameStatePresenter>(Lifetime.Singleton);

            RegisterScreens(builder);
            
            RegisterLevel(builder);
            RegisterBoard(builder);
            RegisterMove(builder);
            builder.Register<IInitialize, GamePlayPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<IInitialize, PowerUpPresenter>(Lifetime.Singleton).AsSelf();
            RegisterGoal(builder);

            builder.Register<IInitialize, MatchMediator>(Lifetime.Singleton).AsSelf();
            
            builder.Register<CellPrefabCreator>(Lifetime.Singleton);
            builder.Register<CellCreator>(Lifetime.Singleton);
        }

        private void RegisterScreens(IContainerBuilder builder)
        {
            builder.RegisterInstance(homeScreenResources);
            builder.Register<IScreenPresenter, HomeScreenPresenter>(Lifetime.Singleton).AsSelf();
            ;

            builder.RegisterInstance(gameScreenResources);
            builder.Register<IScreenPresenter, GameScreenPresenter>(Lifetime.Singleton).AsSelf();

            builder.RegisterInstance(levelEndScreenResources);
            builder.Register<IScreenPresenter, LevelEndScreenPresenter>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterBoard(IContainerBuilder builder)
        {
            builder.Register<BoardModel>(Lifetime.Singleton);
            builder.Register<IInitialize ,BoardPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<IInitialize, BoardFillPresenter>(Lifetime.Singleton).AsSelf();
            
            builder.RegisterInstance(boardCreationData);
            builder.RegisterInstance(boardResources);
        }        
        
        private void RegisterGoal(IContainerBuilder builder)
        {
            builder.Register<GoalPresenter>(Lifetime.Singleton);
            builder.RegisterInstance(goalResources);
        }

        private void RegisterLevel(IContainerBuilder builder)
        {
            builder.Register<IInitialize, LevelCreationPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelPresenter>(Lifetime.Singleton);
            builder.RegisterInstance(levelContainer);
        }

        private void RegisterMove(IContainerBuilder builder)
        {
            builder.Register<MovePresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterInstance(moveResources);
        }
    }
}