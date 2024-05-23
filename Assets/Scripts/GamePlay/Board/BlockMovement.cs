using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using UnityEngine;

namespace GamePlay.Board
{
    public class BlockMovement
    {
        private readonly BlockMovementData _movementData;

        public BlockMovement(BlockMovementData movementData)
        {
            _movementData = movementData;
        }

        public void Fall(Transform transform, Vector3 targetPosition, bool bounce)
        {
            var ease = _movementData.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _movementData.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() => { transform.DOJump(transform.position, _movementData.bouncePower, 1, _movementData.bounceDuration); });
        }

        public async UniTask FallDelayed(Transform transform, Vector3 targetPosition, bool bounce, int delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay * _movementData.delayFactor));

            var ease = _movementData.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _movementData.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() => { transform.DOJump(transform.position, _movementData.bouncePower, 1, _movementData.bounceDuration); });

            await fall.AsyncWaitForCompletion().AsUniTask();
        }

        public Tween Blast(Transform transform, Vector3 targetPosition)
        {
            var tween = DOTween.Sequence();
            tween.Append(transform.DOMove(targetPosition, _movementData.blastDuration).SetEase(Ease.InBack));
            tween.Append(transform.DOScale(0f, 0.1f).SetEase(Ease.InBack, 2f));

            return tween;
        }

        public Tween Blast(Transform transform)
        {
            var tween = transform.DOScale(0f, 0.25f).SetEase(Ease.InBack, 2f);
            return tween;
        }
    }
}