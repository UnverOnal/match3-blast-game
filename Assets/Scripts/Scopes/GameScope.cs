using Board;
using Board.BoardCreation;
using GameManagement;
using GamePlay;
using GameState;
using Level;
using UI;
using UI.Screens;
using UI.Screens.Game;
using UI.Screens.Home;
using Ui.Screens.LevelEnd;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IInitializable = GameManagement.IInitializable;

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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameSceneManager>();
            builder.Register<UiManager>(Lifetime.Singleton).AsSelf().As<IInitializable>();
            
            RegisterScreens(builder);
            RegisterBoard(builder);
            RegisterLevel(builder);
            builder.Register<GamePlayPresenter>(Lifetime.Singleton).AsSelf().As<IInitializable>();

            builder.Register<GameStatePresenter>(Lifetime.Singleton);
        }
        
        private void RegisterScreens(IContainerBuilder builder)
        {
            builder.RegisterInstance(homeScreenResources);
            builder.Register<HomeScreenPresenter>(Lifetime.Singleton).AsSelf().As<IScreenPresenter>();;

            builder.RegisterInstance(gameScreenResources);
            builder.Register<GameScreenPresenter>(Lifetime.Singleton).AsSelf().As<IScreenPresenter>();

            builder.RegisterInstance(levelEndScreenResources);
            builder.Register<LevelEndScreenPresenter>(Lifetime.Singleton).AsSelf().As<IScreenPresenter>();
        }

        private void RegisterBoard(IContainerBuilder builder)
        {
            builder.Register<LevelCreationPresenter>(Lifetime.Singleton).AsSelf().As<IInitializable>();
            builder.Register<BoardModel>(Lifetime.Singleton);
            builder.Register<BoardPresenter>(Lifetime.Singleton).AsSelf().As<IInitializable>();
            builder.RegisterInstance(boardCreationData);
            builder.RegisterInstance(boardResources);
            builder.Register<BlockCreator>(Lifetime.Singleton);
        }

        private void RegisterLevel(IContainerBuilder builder)
        {
            builder.Register<LevelPresenter>(Lifetime.Singleton);
            builder.RegisterInstance(levelContainer);
        }
    }
}
