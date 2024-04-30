using GamePlay.CellManagement;
using MoveManagement;
using UnityEngine;

namespace Level.LevelCounter
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        //GOAL var

        [Header("BoardSize")] public int width;
        public int height;

        [Header("BlockData")] public LevelBlockData[] blockData;

        [Header("ObstacleData")] public LevelObstacleData[] obstacleData;

        [Header("PowerUpData")] public LevelPowerUpData[] powerUpData;

        [Header("MoveData")] public MoveData moveData;
    }
}