using System;
using PowerUpManagement;

namespace GamePlay.CellManagement
{
    public enum CellType
    {
        Block,
        Obstacle,
        PowerUp
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
        [NonSerialized] public int amount;
        public PowerUpType type;
        public ImpactArea impactArea;
        public int creationThreshold;
    }
}