using System;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        //GOAL var

        public BoardSize boardSize;

        [Serializable]
        public struct BlockData
        {
            public CellType type;
            public int amount;
        }
        public BlockData[] blockData;

        public int moveCount;
        public float duration = 60f;

        public int[] obstacleHealths;
    }

    public enum CellType
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
    public struct BoardSize
    {
        public int x;
        public int y;
    }
}