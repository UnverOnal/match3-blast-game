using UnityEngine;

namespace PowerUpManagement.PowerUpTypes
{
    public class PowerUp
    {
        protected PowerUpType type;
        protected GameObject gameObject;
        protected int threshold;
        
        public void SetData(PowerUpCreationData data)
        {
            gameObject = data.prefab;
            threshold = data.creationThreshold;
            type = data.type;
        }

        public void Reset()
        {
            gameObject = null;
            threshold = int.MaxValue;
        }
    }
}
