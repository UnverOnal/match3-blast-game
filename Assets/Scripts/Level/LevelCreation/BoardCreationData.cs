using System;
using GamePlay.CellManagement;
using PowerUpManagement;
using UnityEngine;

namespace Level.LevelCreation
{
    [CreateAssetMenu(fileName = "BoardCreationData", menuName = "ScriptableObjects/BoardCreationData")]
    public class BoardCreationData : ScriptableObject
    {
        public BlockCreationData[] blockCreationData;
        public ObstacleCreationData[] obstacleCreationData;

        public Sprite background;

        public PowerUpCreationData[] powerUps;
    }
    
    [Serializable]public struct BlockCreationData
    {
        public BlockType type;
        public GameObject prefab;
    }
    
    [Serializable]public struct ObstacleCreationData
    {
        public ObstacleType type;
        public GameObject prefab;
    }
}
