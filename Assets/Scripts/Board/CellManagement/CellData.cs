using Level;
using UnityEngine;

namespace Board.CellManagement
{
    public struct CellData
    {
        public CellData(BoardLocation location, GameObject gameObject, CellType cellType)
        {
            this.location = location;
            this.gameObject = gameObject;
            this.cellType = cellType;
        }
        
        public BoardLocation location;
        public GameObject gameObject;
        public CellType cellType;
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
