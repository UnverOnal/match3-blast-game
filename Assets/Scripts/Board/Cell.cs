using UnityEngine;

namespace Board
{
    public class Cell
    {
        public Vector3 Position => _gameObject.transform.position;
        public Vector2 Scale => _gameObject.transform.localScale;
        public Vector2 SpriteSize { get; private set; } 
        
        private GameObject _gameObject;
        private Sprite _sprite;
        private BoardLocation _location;

        public void SetGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
            _sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            SpriteSize = _sprite.rect.size * _gameObject.transform.localScale;
        }

        public void SetLocation(BoardLocation location) => _location = location;
        
        public void Reset(){}
    }
}
