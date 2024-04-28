using Level.LevelCounter;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public abstract class CellBase
    {
        public Vector3 Position => GameObject.transform.position;
        public Vector2 Extents => _sprite.bounds.extents * (Vector2)GameObject.transform.localScale;
        
        public GameObject GameObject { get; private set; }
        public CellType CellType { get; private set; }

        public BoardLocation Location { get; private set; }
        
        private Sprite _sprite;

        public void SetCellData(CellData cellData)
        {
            SetLocation(cellData.location);
            CellType = cellData.blockData.type;
            
            GameObject = cellData.gameObject;
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
}
