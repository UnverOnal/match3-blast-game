using System.Collections.Generic;
using Ui.Animation.Transition.TransitionData;

namespace Ui.Animation.Transition.TransitionAnimations
{
    public abstract class UiTransition
    {
        protected readonly IEnumerable<UiTransitionData> uiTransitionData;

        protected UiTransition(IEnumerable<UiTransitionData> uiTransitionData)
        {
            this.uiTransitionData = uiTransitionData;
        }
    }
}
