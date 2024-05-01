using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameManagement.LifeCycle;
using GamePlay.Board;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.Mediator;
using Level.LevelCounter;
using Level.LevelCreation;
using PowerUpManagement.PowerUpTypes;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace PowerUpManagement
{
    public class PowerUpPresenter : Colleague, IInitialize, IDisposable
    {
        [Inject] private BoardModel _boardModel;
        [Inject] private BoardFillPresenter _fillPresenter;
        [Inject] private IInputService _inputService;

        private readonly PowerUpView _powerUpView;

        private readonly LevelPresenter _levelPresenter;
        private readonly CellCreator _cellCreator;

        [Inject]
        public PowerUpPresenter(BoardCreationData boardCreationData,
            LevelPresenter levelPresenter, CellCreator cellCreator)
        {
            _levelPresenter = levelPresenter;
            _cellCreator = cellCreator;
            _powerUpView = new PowerUpView(cellCreator, levelPresenter, boardCreationData);
        }

        public void Initialize()
        {
            _powerUpView.OnPowerUpCreated += _cellCreator.AddCell;
        }

        public void CreatePowerUp(CellGroup selectedGroup, BoardLocation selectedBlockLocation)
        {
            var type = GetPowerUpType(selectedGroup.blocks.Count);
            if (type == CellType.None)
                return;

            var bottomLocations = selectedGroup.bottomLocations;
            var location = bottomLocations[selectedBlockLocation.x];
            _powerUpView.CreatePowerUp(type, location);
        }

        public async UniTask Explode(GameObject gameObject)
        {
            var powerUp = (PowerUp)_boardModel.GetCell(gameObject);
            powerUp.OnExplode += _cellCreator.RemoveCell;

            _inputService.IgnoreInput(true);
            await powerUp.Explode(_boardModel.Cells, _fillPresenter);
            _inputService.IgnoreInput(false);
            
            powerUp.Reset();
            powerUp.OnExplode -= _cellCreator.RemoveCell;
        }

        private CellType GetPowerUpType(int blastedBlockCount)
        {
            var datas = _levelPresenter.GetCurrentLevelData().powerUpData;
            var orderedDatas = datas.OrderByDescending(data => data.creationThreshold);

            foreach (var data in orderedDatas)
                if (blastedBlockCount > data.creationThreshold)
                    return data.cellType;

            return CellType.None;
        }

        public void Dispose()
        {
            _powerUpView.OnPowerUpCreated -= _cellCreator.AddCell;
        }
    }
}