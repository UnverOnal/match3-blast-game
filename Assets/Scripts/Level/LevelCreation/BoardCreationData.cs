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

        public Sprite background;
    }
    
    [Serializable]public struct CellAssetData
    {
        public CellType type;
        public GameObject prefab;
    }
}
