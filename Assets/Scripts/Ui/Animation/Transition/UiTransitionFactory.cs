using System;
using System.Collections.Generic;
using Ui.Animation.Transition.TransitionAnimations;
using Ui.Animation.Transition.TransitionData;

namespace Ui.Animation.Transition
{
    public enum UiTransitionType
    {
        Fade,
        ScaleUp,
        Slide,
        TextReveal
    }

    public class UiTransitionFactory
    {
        public IUiTransition GetTransition(UiTransitionType type, IEnumerable<UiTransitionData> uiTransitionData)
        {
            return type switch
            {
                UiTransitionType.Fade => new Fade(uiTransitionData),
                UiTransitionType.ScaleUp => new ScaleUp(uiTransitionData),
                UiTransitionType.Slide => new Slide(uiTransitionData),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}