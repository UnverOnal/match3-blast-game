using GameManagement;
using GameState;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameSceneManager>();
            builder.Register<UiManager>(Lifetime.Singleton);
            
            RegisterScreens(builder);
            
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
    }
}
