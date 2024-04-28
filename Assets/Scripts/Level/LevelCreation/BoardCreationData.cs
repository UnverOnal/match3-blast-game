using System;
using Level.LevelCounter;
using UnityEngine;

namespace Level.LevelCreation
{
    [CreateAssetMenu(fileName = "BoardCreationData", menuName = "ScriptableObjects/BoardCreationData")]
    public class BoardCreationData : ScriptableObject
    {
        [Serializable]public struct BlockCreationData
        {
            public CellType type;
            public GameObject prefab;
        }

        public BlockCreationData[] blockCreationData;

        public Sprite background;
    }
}
