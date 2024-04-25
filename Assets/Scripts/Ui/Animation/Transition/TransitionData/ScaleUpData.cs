using System;
using DG.Tweening;

namespace Ui.Animation.Transition.TransitionData
{
    [Serializable]
    public class ScaleUpData : UiTransitionData
    {
        public Ease ease;
        public float overshoot;
    }
}
