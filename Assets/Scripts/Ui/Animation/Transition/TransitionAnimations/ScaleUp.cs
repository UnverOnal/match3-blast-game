using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ui.Animation.Transition.TransitionData;
using UnityEngine;

namespace Ui.Animation.Transition.TransitionAnimations
{
    public class ScaleUp :UiTransition, IUiTransition
    {
        private readonly IEnumerable<ScaleUpData> _scaleUpData;

        public ScaleUp(IEnumerable<UiTransitionData> uiTransitionData) : base(uiTransitionData)
        {
            _scaleUpData = this.uiTransitionData.Cast<ScaleUpData>();
        }

        public async UniTask Enable()
        {
            foreach (var data in _scaleUpData)
            {
                var transform = data.gameObject.transform;
                var scale = transform.localScale;
                transform.localScale = Vector3.zero;
                
                if (data.delayTime > 0f)
                    await UniTask.Delay(TimeSpan.FromSeconds(data.delayTime));
                transform.DOScale(scale, data.duration).SetEase(data.ease, data.overshoot);
            }
        }

        public UniTask Disable()
        {
            return UniTask.CompletedTask;
        }
    }
}