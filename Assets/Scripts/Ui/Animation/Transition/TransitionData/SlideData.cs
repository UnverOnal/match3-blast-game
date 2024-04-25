using System;
using DG.Tweening;
using UnityEngine;

namespace Ui.Animation.Transition.TransitionData
{
    [Serializable]
    public class SlideData : UiTransitionData
    {
        public Vector3 direction;
        public Ease ease;
        public float distance;
        public float overshoot;
    }
}
