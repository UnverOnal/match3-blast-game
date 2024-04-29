using System;
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
    }
}