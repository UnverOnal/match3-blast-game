using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneService
{
    public class SceneService : ISceneService
    {
        private readonly SceneDataContainer _sceneDataContainer;

        /// <summary>
        /// Called right after the loading of the scene.
        /// </summary>
        public event Action OnSceneLoaded;
        
        /// <summary>
        /// Called right before the unloading of the scene.
        /// </summary>
        public event Action OnSceneUnloaded;
        
        public event Action<float> OnProgressUpdated;
        
        private SceneData _currentScene;

        public SceneService(SceneDataContainer sceneDataContainer)
        {
            _sceneDataContainer = sceneDataContainer;
        }

        public async void LoadScene(SceneType type)
        {
            var sceneData = _sceneDataContainer.GetSceneData(type);
            
            if (_currentScene != null)
                UnloadSceneAsync(_currentScene);

            _currentScene = sceneData;

            if (sceneData.ShowLoadingScene)
            {
                var asyncOperation =
                    SceneManager.LoadSceneAsync(sceneData.loadingScene.type.ToString(), LoadSceneMode.Additive);
                await UniTask.WaitUntil(() => asyncOperation.isDone);
            }

            LoadSceneAsync(sceneData);
        }
        
        private async void LoadSceneAsync(SceneData sceneData)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneData.type.ToString(), LoadSceneMode.Additive);

            while (!asyncOperation.isDone)
            {
                var progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                OnProgressUpdated?.Invoke(progress);

                await UniTask.Yield();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneData.type.ToString()));
            
            OnSceneLoaded?.Invoke();

            if (sceneData.ShowLoadingScene)
                UnloadSceneAsync(sceneData.loadingScene);
        }

        private void UnloadSceneAsync(SceneData sceneData)
        {
            OnSceneUnloaded?.Invoke();
            
            var sceneToUnload = SceneManager.GetSceneByName(sceneData.type.ToString());
            if (sceneToUnload.isLoaded)
                SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}