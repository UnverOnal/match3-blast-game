using UnityEngine;

namespace GameManagement
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Camera")]
        public float cameraSizeOffset;
        public Vector3 cameraPositionOffset;
    }
}