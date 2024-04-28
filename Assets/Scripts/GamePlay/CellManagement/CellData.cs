using Level.LevelCounter;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public struct CellData
    {
        public CellData(BoardLocation location, GameObject gameObject, LevelData.BlockData blockData)
        {
            this.location = location;
            this.gameObject = gameObject;
            // this.cellType = cellType;
            this.blockData = blockData;
        }
        
        public BoardLocation location;
        public GameObject gameObject;
        // public CellType cellType;
        public LevelData.BlockData blockData;
    }
    
    public struct BoardLocation
    {
        public readonly int x;
        public readonly int y;

        public BoardLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
