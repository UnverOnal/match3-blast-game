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
        }

        public void SetData(PowerUpCreationData creationData)
        {
            threshold = creationData.creationThreshold;
            type = creationData.type;
            impactArea = creationData.impactArea;
        }

        public override void Reset()
        {
            base.Reset();
            threshold = int.MaxValue;
            impactArea = default;
        }
    }
}
