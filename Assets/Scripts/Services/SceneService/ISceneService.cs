using System;

namespace Services.SceneService
{
    public interface ISceneService
    {
        event Action OnSceneLoaded;       
        event Action OnSceneUnloaded;
        event Action<float> OnProgressUpdated;
        void LoadScene(SceneType type);
    }
}