using GameManagement;
using Services.AudioService.Scripts;
using Services.CommandService;
using Services.DataStorageService;
using Services.FileConversionService;
using Services.InputService;
using Services.PoolingService;
using Services.SceneService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class MainScope : LifetimeScope
    {
        [SerializeField] private SceneDataContainer sceneDataContainer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            InstallServices(builder);

            builder.RegisterInstance(sceneDataContainer);
            
            builder.RegisterEntryPoint<GameManager>();
        }

        private void InstallServices(IContainerBuilder builder)
        {
            builder.Register<ISceneService, SceneService>(Lifetime.Singleton);
            builder.Register<IPoolService, PoolService>(Lifetime.Singleton);
            
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>().AsSelf();
            builder.RegisterEntryPoint<InputEntryPoint>();

            builder.Register<IFileConversionService, FileConversionService>(Lifetime.Singleton);

            builder.Register<ICommandService, CommandService>(Lifetime.Singleton);

            builder.Register<IDataStorageService, DataStorageService>(Lifetime.Singleton);
            
            builder.Register<IAudioService, AudioService>(Lifetime.Singleton);
        }
    }
}
