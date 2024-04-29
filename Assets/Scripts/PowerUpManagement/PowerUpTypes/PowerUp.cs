using GamePlay.CellManagement;

namespace PowerUpManagement.PowerUpTypes
{
    public class PowerUp : Cell
    {
        protected PowerUpType type;
        protected int threshold;
        protected ImpactArea impactArea;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var creationData = (LevelPowerUpData)cellCreationData.levelCellData;
            threshold = creationData.creationThreshold;
            type = creationData.type;
            impactArea = creationData.impactArea;
        }

        public override void Reset()
        {
            Location = new BoardLocation();
            threshold = int.MaxValue;
            impactArea = default;
        }
    }
}
