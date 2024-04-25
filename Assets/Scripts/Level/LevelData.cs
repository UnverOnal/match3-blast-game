using System;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        //GOAL var
        
        public GridSize gridSize;
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
        Yellow
    }
    
    [Serializable]
    public struct BlockData
    {
        public BlockType type;
        public int amount;
    }

    [Serializable]
    public struct GridSize
    {
        public int rows;
        public int columns;
    }
}
