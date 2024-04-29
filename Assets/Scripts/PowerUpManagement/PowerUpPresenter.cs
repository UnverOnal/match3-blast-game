using GameManagement;
using GamePlay.Mediator;
using Level.LevelCreation;
using Services.PoolingService;
using VContainer;

namespace PowerUpManagement
{
    public class PowerUpPresenter : Colleague, IInitializable
    {
        private PowerUpCreator _powerUpCreator;

        [Inject]
        public PowerUpPresenter(IPoolService poolService ,BoardCreationData boardCreationData)
        {
            _powerUpCreator = new PowerUpCreator(poolService, boardCreationData.powerUps);
        }

        public void Initialize()
        {
        }

        public void CreatePowerUp()
        {
            
        }
    }
}