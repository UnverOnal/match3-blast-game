using System.Collections.Generic;
using UnityEngine;

namespace Level.LevelCounter
{
    [CreateAssetMenu(fileName = "LevelContainer", menuName = "ScriptableObjects/LevelContainer")]
    public class LevelContainer : ScriptableObject
    {
        public List<LevelData> levels;
    }
}
