
using GamePlay.Board;

namespace GamePlay.CellManagement
{
    public class Obstacle : Cell, IDamageable
    {
        private readonly BlockMovement _blockMovement;
        private int _health;
        
        public Obstacle(BlockMovement blockMovement)
        {
            _blockMovement = blockMovement;
        }

        public bool CanExplode() => _health <= 0;

        public void Damage() => _health--;

        public void Explode() => _blockMovement.Blast(this);

        public BoardLocation GetLocation() => Location;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (LevelObstacleData)cellCreationData.levelCellData;
            _health = cellData.health;
        }
    }
}
