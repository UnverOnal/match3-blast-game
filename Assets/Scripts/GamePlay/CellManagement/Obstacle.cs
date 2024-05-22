using DG.Tweening;
using UnityEngine;

namespace GamePlay.CellManagement
{
    public class Obstacle : Cell, IDamageable
    {
        private int _health;

        public bool CanExplode() => _health <= 0;

        public void Damage() => _health--;

        public void Explode() => Destroy();

        public BoardLocation GetLocation() => Location;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (LevelObstacleData)cellCreationData.levelCellData;
            _health = cellData.health;
        }
    }
}
