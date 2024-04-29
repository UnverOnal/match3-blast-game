using DG.Tweening;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public class Obstacle : Cell, IDamageable
    {
        public ObstacleType ObstacleType { get; private set; }

        private int _health;

        public bool CanExplode() => _health <= 0;

        public void Damage() => _health--;
        
        public void Explode()
        {
            var transform = GameObject.transform;
            var originalScale = transform.localScale;
            transform.DOScale(0f, 0.35f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GameObject.SetActive(false);
                transform.localScale = originalScale;
            });
        }

        public BoardLocation GetLocation() => Location;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (LevelObstacleData)cellCreationData.levelCellData;
            ObstacleType = cellData.type;
            _health = cellData.health;
        }
    }
}
