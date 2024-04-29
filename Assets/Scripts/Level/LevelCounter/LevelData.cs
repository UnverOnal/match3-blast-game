using GamePlay.CellManagement;
using UnityEngine;

namespace Level.LevelCounter
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        //GOAL var

        [Header("BoardSize")]
        public int width;
        public int height;
        
        [Header("BlockData")]
        public BlockData[] blockData;

        [Header("ObstacleData")] 
        public ObstacleData[] obstacleData;
        
        [Header("MoveData")]
        public int moveCount;
        public float duration = 60f;
    }
}