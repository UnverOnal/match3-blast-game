namespace GamePlay.CellManagement
{
    public class Obstacle : Cell
    {
        public ObstacleType ObstacleType { get; private set; }
        public bool CanExplode => _health <= 0;
        
        private int _health;

        public void Damage() => _health--;
        
        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var cellData = (LevelObstacleData)cellCreationData.levelCellData;
            ObstacleType = cellData.type;
        }
    }
}
