using UnityEngine;

namespace Board
{
    public class Cell
    {
        public Vector3 Position => _gameObject.transform.position;
        public Vector2 Scale => _gameObject.transform.localScale;
        public Vector2 Extents => _sprite.bounds.extents;
        
        private GameObject _gameObject;
        private BoardLocation _location;
        private Sprite _sprite;

        public void SetGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
            _sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        public void SetLocation(BoardLocation location) => _location = location;
        
        public void Reset(){}
    }
}
