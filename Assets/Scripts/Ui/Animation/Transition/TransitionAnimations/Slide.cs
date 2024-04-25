using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ui.Animation.Transition.TransitionData;
using UnityEngine;

namespace Ui.Animation.Transition.TransitionAnimations
{
    public class Slide : UiTransition, IUiTransition
    {
        private readonly IEnumerable<SlideData> _slideData;

        public Slide(IEnumerable<UiTransitionData> uiTransitionData) : base(uiTransitionData)
        {
            _slideData = this.uiTransitionData.Cast<SlideData>();
        }

        public async UniTask Enable()
        {
            var tasks = new List<UniTask>();
            foreach (var data in _slideData)
            {
                var direction = data.direction.normalized;
                var targetPosition = data.gameObject.transform.position;
                var startingPosition = targetPosition - direction * data.distance;
                
                var task = SlideTo(data, startingPosition, targetPosition);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public UniTask Disable()
        {
            return UniTask.CompletedTask;
        }

        private async UniTask SlideTo(SlideData data, Vector3 startingPosition, Vector3 position)
        {

            var transform = data.gameObject.transform;
            transform.position = startingPosition;
                
            if (data.delayTime > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(data.delayTime));

            transform.DOMove(position, data.duration).SetEase(data.ease, data.overshoot);
        }
    }
}