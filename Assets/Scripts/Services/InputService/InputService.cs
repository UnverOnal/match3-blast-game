using System;
using UnityEngine;

namespace Services.InputService
{
    public class InputService : IInputService
    {
        public event Action<GameObject> OnItemPicked; 
        public event Action OnItemReleased;
        
        private GameObject _pickedGameObject;
        
        private readonly InputTracker _inputTracker;

        public InputService()
        {
            _inputTracker = new InputTracker(Camera.main);
        }
        
        public void Update()
        {
            if (_inputTracker.IgnoreInput)
                return;
            
            _inputTracker.Track();
            
            if(_inputTracker.IsPointerDown)
                Pick();
            
            if (_inputTracker.IsPointerUp && _pickedGameObject != null)
            {
                OnItemReleased?.Invoke();
                _pickedGameObject = null;
            }
        }

        public void IgnoreInput(bool ignore)
        {
            _inputTracker.IgnoreInput = ignore;
        }

        ///Picks item selected by using tap/click. Doesn't run if OnItemPicked event is not subscribed.
        private void Pick()
        {
            if(OnItemPicked == null)
                return;

            _pickedGameObject = _inputTracker.GetSelectedGameObject();
            if(_pickedGameObject != null)
                OnItemPicked.Invoke(_pickedGameObject);
        }
        
        public int GetSwipe(float sensitivity)
        {
            var input = Vector3.Dot(Vector3.right, _inputTracker.DeltaPosition) * sensitivity / Screen.width;

            var swipeInput = (int)Mathf.Clamp(input, -1.1f, 1.1f);

            return swipeInput;
        }

        public Vector2 GetDragInput(float sensitivity)
        {
            var dragInput = new Vector2
            {
                x = GetDragOnSingleAxis(Vector3.right, sensitivity),
                y = GetDragOnSingleAxis(Vector3.up, sensitivity)
            };

            return dragInput;
        }

        private float GetDragOnSingleAxis(Vector3 axis, float sensitivity)
        {
            var input = Vector3.Dot(axis, _inputTracker.DeltaPosition) * sensitivity / Screen.width;
            return input;
        }
    }
}