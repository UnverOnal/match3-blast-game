using System;
using GamePlay.CellManagement;
using UnityEngine;

namespace PowerUpManagement
{
    public enum PowerUpType
    {
        Bomb,
        Rocket
    }

    [Serializable]
    public struct PowerUpCreationData
    {
        public PowerUpType type;
        public int creationThreshold;
        public GameObject prefab;
        public ImpactArea impactArea;
    }

    [Serializable]
    public struct ImpactArea
    {
        public int x;
        public int y;
    }
}