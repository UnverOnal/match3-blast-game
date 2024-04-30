using System;
using PowerUpManagement;

namespace GamePlay.CellManagement
{
    public enum CellType
    {
        Block,
        Obstacle,
        PowerUp,
        None
    }

    public enum BlockType
    {
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Yellow
    }

    public enum ObstacleType
    {
        Stone
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
    public class LevelBlockData : LevelCellData
    {
        public BlockType type;
    }

    [Serializable]
    public class LevelObstacleData : LevelCellData
    {
        public ObstacleType type;
        public int health;
    }

    [Serializable]
    public class LevelPowerUpData : LevelCellData
    {
        public PowerUpType type;
        public int creationThreshold;
    }
}