using System.Collections.Generic;
using UnityEngine;

namespace Services.SceneService
{
    [CreateAssetMenu(fileName = "SceneDataContainer", menuName = "ScriptableObjects/SceneDataContainer")]
    public class SceneDataContainer : ScriptableObject
    {
        [SerializeField] private List<SceneData> sceneDatas;

        public SceneData GetSceneData(SceneType type)
        {
            for (int i = 0; i < sceneDatas.Count; i++)
            {
                var sceneData = sceneDatas[i];
                if(sceneData.type == type)
                    return sceneData;
            }
            return null;
        }
    }
}
