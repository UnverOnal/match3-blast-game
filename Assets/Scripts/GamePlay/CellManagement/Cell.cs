using UnityEngine;

namespace GamePlay.CellManagement
{
    public abstract class Cell
    {
        public Vector3 Position => GameObject.transform.position;
        public Vector2 Extents => _sprite.bounds.extents * (Vector2)GameObject.transform.localScale;
        
        public GameObject GameObject { get; private set; }
        public CellType CellType { get; private set; }

        public BoardLocation Location { get; private set; }
        
        private Sprite _sprite;

        public virtual void SetData(CellCreationData cellCreationData)
        {
            SetLocation(cellCreationData.location);
            CellType = cellCreationData.levelCellData.cellType;
            
            GameObject = cellCreationData.gameObject;
            _sprite = GameObject.GetComponent<SpriteRenderer>().sprite;
        }

        public void Reset()
        {
            GameObject = null;
            Location = new BoardLocation();
            _sprite = null;
        }

        public void SetLocation(BoardLocation boardLocation)
        {
            Location = boardLocation;
        }
    }
    
    public struct CellCreationData
    {
        public CellCreationData(BoardLocation location, GameObject gameObject, LevelCellData levelCellData)
        {
            this.location = location;
            this.gameObject = gameObject;
            this.levelCellData = levelCellData;
        }

        public BoardLocation location;
        public GameObject gameObject;
        public LevelCellData levelCellData;
    }

    public struct BoardLocation
    {
        public int x;
        public int y;

        public BoardLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
