using System;
using Level.LevelCounter;

namespace GamePlay.CellManagement
{
    public enum CellType
    {
        Obstacle,
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Yellow,
        Bomb,
        Rocket,
        None
    }

    [Serializable]
    public class LevelCellData
    {
        public CellType cellType;

        public object Clone() => MemberwiseClone();
    }
    
    [Serializable]
    public class LevelBlockData : LevelCellData
    {
        public int amount;
    }

    [Serializable]
    public class LevelObstacleData : LevelCellData
    {
        public BoardLocation location;
        public int health;
    }

    [Serializable]
    public class LevelPowerUpData : LevelCellData
    {
        public int creationThreshold;
    }
}