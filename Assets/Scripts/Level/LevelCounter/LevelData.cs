using GamePlay.CellManagement;
using GoalManagement;
using MoveManagement;
using UnityEngine;

namespace Level.LevelCounter
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        [Header("BoardSize")] public CounterData[] counterGoalDatas;

        [Header("BoardSize")] public int width;
        public int height;

        [Header("BlockData")] public LevelCellData[] blockData;

        [Header("ObstacleData")] public LevelObstacleData[] obstacleData;

        [Header("PowerUpData")] public LevelPowerUpData[] powerUpData;

        [Header("MoveData")] public MoveData moveData;
    }
}