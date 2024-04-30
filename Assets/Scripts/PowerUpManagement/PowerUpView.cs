using System;
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

        public PowerUpView(PowerUpCreator powerUpCreator, LevelPresenter levelPresenter,
            BoardCreationData boardCreationData)
        {
            _powerUpCreator = powerUpCreator;
            _levelPresenter = levelPresenter;
            _boardCreationData = boardCreationData;

            _powerUpParent = new GameObject("PowerUps");
        }

        public void CreatePowerUp(PowerUpType tempType, BoardLocation location)
        {
            var powerUp = _powerUpCreator.GetPowerUp(tempType);

            var prefab = SpawnPowerUp(powerUp, new Vector3(location.x, location.y), tempType);
            var creationData = new CellCreationData(location, prefab, GetLevelPowerUpData(tempType));
            powerUp.SetData(creationData);

            OnPowerUpCreated?.Invoke(powerUp);
        }

        private GameObject GetPrefab(PowerUpType type)
        {
            var powerUpData = _boardCreationData.powerUps;
            for (var i = 0; i < powerUpData.Length; i++)
            {
                var data = powerUpData[i];
                if (data.type == type)
                    return data.prefab;
            }

            return null;
        }

        private LevelPowerUpData GetLevelPowerUpData(PowerUpType type)
        {
            var levelPowerUpDatas = _levelPresenter.GetCurrentLevelData().powerUpData;
            for (var i = 0; i < levelPowerUpDatas.Length; i++)
            {
                var data = levelPowerUpDatas[i];
                if (data.type == type)
                    return data;
            }

            return null;
        }

        private GameObject SpawnPowerUp(PowerUp powerUp, Vector3 position, PowerUpType type)
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