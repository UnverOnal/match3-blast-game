using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using UnityEngine;

namespace GamePlay.Board
{
    public class BlockFall
    {
        private readonly BlockMovementData _movementData;

        public BlockFall(BlockMovementData movementData)
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
        }
    }
}