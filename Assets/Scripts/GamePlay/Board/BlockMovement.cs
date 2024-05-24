using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using GamePlay.CellManagement;
using GamePlay.ParticleManagement;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BlockMovement
    {
        [Inject] private readonly ParticleManager _particleManager;
        
        private readonly BlockMovementData _movementData;

        public BlockMovement(GameSettings gameSettings)
        {
            _movementData = gameSettings.blockMovementData;
        }

        public void Fall(Transform transform, Vector3 targetPosition, bool bounce)
        {
            var ease = _movementData.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _movementData.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() =>
                {
                    transform.DOJump(transform.position, _movementData.bouncePower, 1,
                        _movementData.bounceDuration);
                });
        }

        public async UniTask FallDelayed(Transform transform, Vector3 targetPosition, bool bounce, int delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay * _movementData.delayFactor));

            var ease = _movementData.fallEase;
            var duration = Mathf.Abs(transform.position.y - targetPosition.y) / _movementData.fallSpeed;
            var fall = transform.DOMove(targetPosition, duration).SetEase(ease);

            if (bounce)
                fall.OnComplete(() =>
                {
                    transform.DOJump(transform.position, _movementData.bouncePower, 1,
                        _movementData.bounceDuration);
                });

            await fall.AsyncWaitForCompletion().AsUniTask();
        }

        public Tween Blast(Cell cell, Vector3 targetPosition)
        {
            var transform = cell.GameObject.transform;
            
            var tween = DOTween.Sequence();
            tween.Append(transform.DOMove(targetPosition, _movementData.blastDuration).SetEase(Ease.InBack));
            tween.Append(transform.DOScale(0f, 0.1f).SetEase(Ease.InBack, 2f));

            return tween;
        }

        public Tween Blast(Cell cell)
        {
            var transform = cell.GameObject.transform;
            var type = GetParticleType(cell.CellType);
            _particleManager.Play(type, transform.position + Vector3.back);
            var tween =
                transform.DOScale(0f, 0.25f).SetEase(Ease.InBack, 2f);

            return tween;
        }

        private ParticleType GetParticleType(CellType cellType)
        {
            var particleType = cellType switch
            {
                CellType.Obstacle => ParticleType.ObstacleExplosion,
                CellType.Blue => ParticleType.BlueExplosion,
                CellType.Green => ParticleType.GreenExplosion,
                CellType.Orange => ParticleType.OrangeExplosion,
                CellType.Purple => ParticleType.PurpleExplosion,
                CellType.Red => ParticleType.RedExplosion,
                CellType.Yellow => ParticleType.YellowExplosion,
                CellType.Bomb => ParticleType.BombExplosion,
                CellType.Rocket => ParticleType.RocketExplosion,
                _ => ParticleType.None
            };

            return particleType;
        }
    }
}