using UnityEngine;

namespace Services.SceneService
{
    public enum SceneType
    {
        MenuScene,
        LoadingScene,
        GameScene
    }

    [CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData")]
    public class SceneData : ScriptableObject
    {
        public SceneType type;
        public SceneData loadingScene;

        public bool ShowLoadingScene => loadingScene != null;
    }
}
