using System;

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
        Yellow,
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

        public object Clone() => MemberwiseClone();
    }
    
    [Serializable]
    public class BlockData : LevelCellData
    {
        public BlockType type;
    }

    [Serializable]
    public class ObstacleData : LevelCellData
    {
        public ObstacleType type;
        public int health;
    }
}