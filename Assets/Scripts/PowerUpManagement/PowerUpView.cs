using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GamePlay.CellManagement;
using Level.LevelCounter;
using Level.LevelCreation;
using PowerUpManagement.PowerUpTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PowerUpManagement
{
    public class PowerUpView
    {
        public event Action<Cell> OnPowerUpCreated;

        private readonly PowerUpCreator _powerUpCreator;
        private readonly LevelPresenter _levelPresenter;
        private readonly BoardCreationData _boardCreationData;

        private readonly GameObject _powerUpParent;
        private IEnumerable<CellAssetData> _powerUpData;

        public PowerUpView(PowerUpCreator powerUpCreator, LevelPresenter levelPresenter,
            BoardCreationData boardCreationData)
        {
            _powerUpCreator = powerUpCreator;
            _levelPresenter = levelPresenter;
            _boardCreationData = boardCreationData;

            _powerUpParent = new GameObject("PowerUps");
            
            _powerUpData =
                _boardCreationData.blockCreationData.Where(data => data.type is CellType.Bomb or CellType.Rocket);
        }

        public void CreatePowerUp(CellType tempType, BoardLocation location)
        {
            var powerUp = _powerUpCreator.GetPowerUp(tempType);

            var prefab = SpawnPowerUp(powerUp, new Vector3(location.x, location.y), tempType);
            var creationData = new CellCreationData(location, prefab, GetLevelPowerUpData(tempType));
            powerUp.SetData(creationData);

            OnPowerUpCreated?.Invoke(powerUp);
        }

        private GameObject GetPrefab(CellType type)
        {
            foreach (var data in _powerUpData)
            {
                if (data.type == type)
                    return data.prefab;
            }

            return null;
        }

        private LevelPowerUpData GetLevelPowerUpData(CellType type)
        {
            var levelPowerUpDatas = _levelPresenter.GetCurrentLevelData().powerUpData;
            for (var i = 0; i < levelPowerUpDatas.Length; i++)
            {
                var data = levelPowerUpDatas[i];
                if (data.cellType == type)
                    return data;
            }

            return null;
        }

        private GameObject SpawnPowerUp(PowerUp powerUp, Vector3 position, CellType type)
        {
            var prefab = powerUp.GameObject == null
                ? Object.Instantiate(GetPrefab(type), parent : _powerUpParent.transform)
                : powerUp.GameObject;
            
            var transform = prefab.transform;
            transform.position = position;
            
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack, 2f);
            return prefab;
        }
    }
}