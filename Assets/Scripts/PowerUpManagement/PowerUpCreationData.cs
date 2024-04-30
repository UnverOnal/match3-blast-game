using System;
using GamePlay.CellManagement;
using UnityEngine;

namespace PowerUpManagement
{
    public enum PowerUpType
    {
        Bomb,
        Rocket,
        None
    }

    [Serializable]
    public struct PowerUpCreationData
    {
        public PowerUpType type;
        public GameObject prefab;
    }
}