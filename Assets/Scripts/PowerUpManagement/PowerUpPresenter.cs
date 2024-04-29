using System;
using System.Linq;
using GameManagement;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using Level.LevelCounter;
using Level.LevelCreation;
using Services.PoolingService;
using VContainer;

namespace PowerUpManagement
{
    public class PowerUpPresenter : Colleague, IInitializable, IDisposable
    {
        [Inject] private BoardModel _boardModel;

        private readonly PowerUpView _powerUpView;

        private readonly BoardCreationData _boardCreationData;
        private readonly LevelPresenter _levelPresenter;
        private readonly PowerUpCreator _powerUpCreator;

        [Inject]
        public PowerUpPresenter(IPoolService poolService, BoardCreationData boardCreationData,
            LevelPresenter levelPresenter)
        {
            _boardCreationData = boardCreationData;
            _levelPresenter = levelPresenter;
            _powerUpCreator = new PowerUpCreator(poolService, boardCreationData.powerUps);
            _powerUpView = new PowerUpView(_powerUpCreator, levelPresenter, boardCreationData);
        }

        public void Initialize()
        {
            _powerUpView.OnPowerUpCreated += _boardModel.AddCell;
        }

        public void CreatePowerUp(CellGroup selectedGroup, BoardLocation selectedBlockLocation)
        {
            var type = GetPowerUpType(selectedGroup.blocks.Count);
            if(type == PowerUpType.None)
                return;
            
            var bottomLocations = selectedGroup.bottomLocations;
            var location = bottomLocations[selectedBlockLocation.x];
            _powerUpView.CreatePowerUp(type, location);
        }

        private PowerUpType GetPowerUpType(int blastedBlockCount)
        {
            var datas = _levelPresenter.GetCurrentLevelData().powerUpData;
            var orderedDatas = datas.OrderByDescending(data => data.creationThreshold);
            
            foreach (var data in orderedDatas)
            {
                if (blastedBlockCount > data.creationThreshold)
                    return data.type;
            }

            return PowerUpType.None;
        }

        public void Dispose()
        {
            _powerUpView.OnPowerUpCreated -= _boardModel.AddCell;
        }
    }
}