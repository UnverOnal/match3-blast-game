using System;
using UnityEngine;

namespace Services.InputService
{
    public interface IInputService
    {
        public void IgnoreInput(bool ignore);
        public int GetSwipe(float sensitivity);
        public Vector2 GetDragInput(float sensitivity);
        public event Action<GameObject> OnItemPicked; 
        public event Action OnItemReleased;
    }
}
