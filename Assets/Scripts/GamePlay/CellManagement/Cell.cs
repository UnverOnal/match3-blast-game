using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public abstract class Cell
    {
        public Vector3 Position => GameObject.transform.position;
        public Vector2 Extents => _sprite.bounds.extents * (Vector2)GameObject.transform.localScale;

        public GameObject GameObject { get; private set; }
        public CellType CellType { get; private set; }

        public BoardLocation Location { get; protected set; }

        protected SpriteRenderer spriteRenderer;
        private Sprite _sprite;
        
        private Vector3 _originalScale;

        public virtual void SetData(CellCreationData cellCreationData)
        {
            SetLocation(cellCreationData.location);
            SetType(cellCreationData.levelCellData.cellType);

            SetGameObject(cellCreationData.gameObject);
            _originalScale = GameObject.transform.localScale;
        }

        private void SetGameObject(GameObject gameObject)
        {
            GameObject = gameObject;
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            _sprite = spriteRenderer.sprite;
        }

        private void SetType(CellType type)
        {
            CellType = type;
        }

        public void SetLocation(BoardLocation boardLocation)
        {
            Location = boardLocation;
        }

        public virtual void Reset()
        {
            GameObject.SetActive(false);
            Location = new BoardLocation();
            GameObject.transform.localScale = _originalScale;
        }

        public Tween Destroy()
        {
            var transform = GameObject.transform;
            var tween = transform.DOScale(0f, 0.25f).SetEase(Ease.InBack, 2f);
            return tween;
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