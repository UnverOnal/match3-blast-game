using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelContainer", menuName = "ScriptableObjects/LevelContainer")]
    public class LevelContainer : ScriptableObject
    {
        public List<LevelData> levels;
    }
}
