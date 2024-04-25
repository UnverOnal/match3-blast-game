using System;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        //GOAL var

        public GridSize gridSize;

        [Serializable]
        public struct BlockData
        {
            public BlockType type;
            public int amount;
        }
        public BlockData[] blockData;

        public int moveCount;
        public float duration = 60f;

        public int[] obstacleHealths;
    }

    public enum BlockType
    {
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Yellow,
        Obstacle
    }

    [Serializable]
    public struct GridSize
    {
        public int rows;
        public int columns;
    }
}