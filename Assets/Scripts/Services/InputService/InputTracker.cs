

using UnityEngine;

namespace Services.InputService
{
    public class InputTracker
    {
        public bool IgnoreInput { get; set; }
        
        public bool IsPointerDown => Input.GetMouseButtonDown(0);
        public bool IsPointerUp => Input.GetMouseButtonUp(0);
        public bool IsPointerHold => Input.GetMouseButton(0);
        
        public Vector3 DeltaPosition => PointerPosition - PointerPreviousPosition;
        public Vector3 PointerPreviousPosition { get; private set; }
        public Vector3 PointerPosition { get; private set; }
        
        private readonly Camera _camera;

        public InputTracker(Camera mainCamera)
        {
            _camera = mainCamera;
        }

        public void Track()
        {
            if (IsPointerDown)
            {
                PointerPosition = Input.mousePosition;
                PointerPreviousPosition = PointerPosition;
                return;
            }
            
            PointerPreviousPosition = PointerPosition;
            if (IsPointerHold)
                PointerPosition = Input.mousePosition;
        }
        
        public GameObject GetSelectedGameObject()
        {
            GameObject pickedGameObject = null; 
            var ray = _camera.ScreenPointToRay(PointerPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                pickedGameObject = hit.transform.gameObject;

            return pickedGameObject;
        }
    }
}
