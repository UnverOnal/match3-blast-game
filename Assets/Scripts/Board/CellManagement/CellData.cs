using Level;
using UnityEngine;

namespace Board.CellManagement
{
    public struct CellData
    {
        public CellData(BoardLocation location, GameObject gameObject, BlockType blockType)
        {
            this.location = location;
            this.gameObject = gameObject;
            this.blockType = blockType;
        }
        
        public BoardLocation location;
        public GameObject gameObject;
        public BlockType blockType;
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
