using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameManagement.LifeCycle;
using GamePlay.Board;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;
using GamePlay.Mediator;
using GamePlay.ParticleManagement;
using Level.LevelCounter;
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
        [Inject] private readonly CellCreator _cellCreator;
        
        private readonly PowerUpView _powerUpView;

        private readonly LevelPresenter _levelPresenter;
        private bool _canUpdateInput;

        [Inject]
        public PowerUpPresenter(LevelPresenter levelPresenter, CellPrefabCreator cellPrefabCreator, ParticleManager particleManager)
        {
            _levelPresenter = levelPresenter;
            _powerUpView = new PowerUpView(cellPrefabCreator, levelPresenter, particleManager);
        }

        public void Initialize()
        {
            _powerUpView.OnPowerUpCreated += CreateCell;
        }
        
        private void CreateCell(CellCreationData data)
        {
            var cell = _cellCreator.CreateCell(data);
            _boardModel.AddCell(cell);
        }

        public void CreatePowerUp(CellGroup selectedGroup, BoardLocation selectedBlockLocation)
        {
            var type = GetPowerUpType(selectedGroup.blocks.Count);
            if (type == CellType.None)
                return;
            
            _powerUpView.CreatePowerUp(type, selectedBlockLocation);
        }

        public async UniTask Explode(GameObject gameObject)
        {
            var powerUp = (PowerUp)_boardModel.GetCell(gameObject);
            powerUp.OnExplode += _boardModel.RemoveCell;

            _inputService.IgnoreInput(true);
            await powerUp.Explode(_boardModel.Cells, _fillPresenter);
            if(_canUpdateInput)
                _inputService.IgnoreInput(false);
            
            powerUp.OnExplode -= _boardModel.RemoveCell;
        }

        public void SetCanIgnoreInput(bool canIgnore)
        {
            _canUpdateInput = canIgnore;
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
            _powerUpView.OnPowerUpCreated -= CreateCell;
        }
    }
}