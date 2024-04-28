using System;
using DG.Tweening;
using UnityEngine;

namespace GameManagement
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Camera")]
        public float cameraSizeOffset;
        public Vector3 cameraPositionOffset;
        public float boundsOffsetFactor;

        [Header("Block Movement")] 
        public BlockMovementData blockMovementData;

    }

    [Serializable]
    public struct BlockMovementData
    {
        public Ease fallEase;
        public float fallSpeed;
        public float delayFactor;
        public float bouncePower;
        public float bounceDuration;
        public float blastDuration;
    }
}