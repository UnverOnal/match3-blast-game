using System;

namespace GamePlay.CellManagement
{
    public enum CellType
    {
        Block,
        Obstacle
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
    public class CellData
    {
        public CellType cellType;
        public int amount;

        public object Clone() => MemberwiseClone();
    }
    
    [Serializable]
    public class BlockData : CellData
    {
        public BlockType type;
    }

    [Serializable]
    public class ObstacleData : CellData
    {
        public ObstacleType type;
        public int health;
    }
}