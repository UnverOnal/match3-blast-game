using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using UnityEngine;

namespace GamePlay.Board
{
    public class BlockFall
    {
        private readonly GameSettings _gameSettings;

        public BlockFall(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void Fall(Transform transform, Vector3 targetPosition, bool bounce)
        {
            var ease = _gameSettings.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _gameSettings.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() => { transform.DOJump(transform.position, _gameSettings.bouncePower, 1, _gameSettings.bounceDuration); });
        }

        public async UniTask FallDelayed(Transform transform, Vector3 targetPosition, bool bounce, int delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay * _gameSettings.delayFactor));

            var ease = _gameSettings.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _gameSettings.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() => { transform.DOJump(transform.position, _gameSettings.bouncePower, 1, _gameSettings.bounceDuration); });
        }
    }
}