using System;
using Level;
using UnityEngine;

namespace Board.BoardCreation
{
    [CreateAssetMenu(fileName = "BoardCreationData", menuName = "ScriptableObjects/BoardCreationData")]
    public class BoardCreationData : ScriptableObject
    {
        [Serializable]public struct BlockData
        {
            public BlockType type;
            public GameObject prefab;
        }

        public BlockData[] blockData;
    }
}
