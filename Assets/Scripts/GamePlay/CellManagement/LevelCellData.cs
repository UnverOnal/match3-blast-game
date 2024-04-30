using System;

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
        public int amount;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    [Serializable]
    public class LevelObstacleData : LevelCellData
    {
        public int health;
    }

    [Serializable]
    public class LevelPowerUpData : LevelCellData
    {
        public int creationThreshold;
    }
}