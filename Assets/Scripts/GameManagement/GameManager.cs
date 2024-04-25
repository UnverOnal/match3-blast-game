using AudioManagement.Scripts.SoundType;
using Services.AudioService.Scripts;
using Services.AudioService.Scripts.ResourceManagement;
using Services.SceneService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameManagement
{
    public class GameManager : IInitializable
    {
        private readonly ISceneService _sceneService;
        private readonly IAudioService _audioService;

        [Inject]
        public GameManager(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
        
            _sceneService.LoadScene(SceneType.GameScene);
        }
    }
}
