using GameManagement;
using GamePlay;
using GamePlay.Board;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
using GameState;
using Level.LevelCounter;
using Level.LevelCreation;
using PowerUpManagement;
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
            builder.Register<IInitializable, UiManager>(Lifetime.Singleton).AsSelf();
            builder.Register<GameStatePresenter>(Lifetime.Singleton);

            RegisterScreens(builder);
            
            RegisterLevel(builder);
            RegisterBoard(builder);
            builder.Register<IInitializable, GamePlayPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<IInitializable, PowerUpPresenter>(Lifetime.Singleton).AsSelf();

            builder.Register<GamePlay.MoveMediator>(Lifetime.Singleton);
            
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
            builder.Register<IInitializable, BoardPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<IInitializable, BoardFillPresenter>(Lifetime.Singleton).AsSelf();
            
            builder.RegisterInstance(boardCreationData);
            builder.RegisterInstance(boardResources);
        }

        private void RegisterLevel(IContainerBuilder builder)
        {
            builder.Register<IInitializable, LevelCreationPresenter>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelPresenter>(Lifetime.Singleton);
            builder.RegisterInstance(levelContainer);
        }
    }
}