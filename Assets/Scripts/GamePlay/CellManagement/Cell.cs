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
            CellType = cellCreationData.cellData.cellType;
            
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
        public CellCreationData(BoardLocation location, GameObject gameObject, CellData cellData)
        {
            this.location = location;
            this.gameObject = gameObject;
            this.cellData = cellData;
        }

        public BoardLocation location;
        public GameObject gameObject;
        public CellData cellData;
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
