using UnityEngine;

namespace Board.CellManagement
{
    public class Cell
    {
        public Vector3 Position => GameObject.transform.position;
        public Vector2 Scale => GameObject.transform.localScale;
        public Vector2 Extents => _sprite.bounds.extents;
        
        public GameObject GameObject { get; private set; }
        
        private BoardLocation _location;
        private Sprite _sprite;

        public void SetGameObject(GameObject gameObject)
        {
            GameObject = gameObject;
            _sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        public void SetLocation(BoardLocation location) => _location = location;
        
        public void Reset(){}
    }
}
