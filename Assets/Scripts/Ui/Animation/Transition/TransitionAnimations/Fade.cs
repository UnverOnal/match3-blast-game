using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ui.Animation.Transition.TransitionData;

namespace Ui.Animation.Transition.TransitionAnimations
{
    public class Fade : UiTransition, IUiTransition
    {
        private readonly IEnumerable<FadeData> _fadeData;

        public Fade(IEnumerable<UiTransitionData> uiTransitionData) : base(uiTransitionData)
        {
            _fadeData = this.uiTransitionData.Cast<FadeData>();
        }

        public async UniTask Enable()
        {
            var tasks = new List<UniTask>();
            foreach (var data in _fadeData)
            {
                var task = FadeIn(data);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask Disable()
        {
            var reversedFadeData = _fadeData.Reverse();

            var tasks = new List<UniTask>();
            foreach (var data in reversedFadeData)
            {
                var task = FadeOut(data);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask FadeIn(FadeData data)
        {
            data.canvasGroup.alpha = 0f;
            if (data.delayTime > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(data.delayTime));

            data.canvasGroup.blocksRaycasts = true;
            data.canvasGroup.interactable = true;

            data.canvasGroup.DOFade(1f, data.duration);
        }

        private async UniTask FadeOut(FadeData data)
        {
            if (data.delayTime > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(data.delayTime));

            data.canvasGroup.blocksRaycasts = false;
            data.canvasGroup.interactable = false;

            data.canvasGroup.DOFade(0f, data.duration).OnComplete(() =>
                data.canvasGroup.alpha = 0f);
        }
    }
}