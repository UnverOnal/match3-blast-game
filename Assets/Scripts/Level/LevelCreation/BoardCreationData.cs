using System;
using GamePlay.CellManagement;
using PowerUpManagement;
using UnityEngine;

namespace Level.LevelCreation
{
    [CreateAssetMenu(fileName = "BoardCreationData", menuName = "ScriptableObjects/BoardCreationData")]
    public class BoardCreationData : ScriptableObject
    {
        public CellAssetData[] blockCreationData;

        public GoalAssetData[] goalAssetData;
        public GameObject goalPrefab;

        public Sprite background;
        
    }
    
    [Serializable]public struct CellAssetData
    {
        public CellType type;
        public GameObject prefab;
    }

    [Serializable]
    public struct GoalAssetData
    {
        public CellType cellType;
        public Sprite sprite;
    }
}
