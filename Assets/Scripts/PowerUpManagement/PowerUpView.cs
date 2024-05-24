using System;
using System.Linq;
using DG.Tweening;
using GamePlay;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;
using GamePlay.ParticleManagement;
using Level.LevelCounter;
using PowerUpManagement.PowerUpTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PowerUpManagement
{
    public class PowerUpView
    {
        public event Action<CellCreationData> OnPowerUpCreated;

        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly LevelPresenter _levelPresenter;
        private readonly ParticleManager _particleManager;

        public PowerUpView(CellPrefabCreator cellPrefabCreator, LevelPresenter levelPresenter, ParticleManager particleManager)
        {
            _cellPrefabCreator = cellPrefabCreator;
            _levelPresenter = levelPresenter;
            _particleManager = particleManager;
        }

        public void CreatePowerUp(CellType cellType, BoardLocation location)
        {
            var prefab = _cellPrefabCreator.Get(cellType);
            var creationData = new CellCreationData(location, prefab, GetLevelPowerUpData(cellType));
            OnPowerUpCreated?.Invoke(creationData);

            SpawnPowerUp(cellType ,prefab, new Vector3(location.x, location.y));
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

        private void SpawnPowerUp(CellType type, GameObject prefab, Vector3 position)
        {
            var transform = prefab.transform;
            transform.position = position;
            
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack, 2f);

            var particleType = type == CellType.Bomb ? ParticleType.BombSpawn : ParticleType.RocketSpawn;
            _particleManager.Play(particleType, position + Vector3.back);
        }
    }
}