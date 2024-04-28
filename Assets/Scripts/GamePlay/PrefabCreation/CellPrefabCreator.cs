using System.Collections.Generic;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation.Factory;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GamePlay.PrefabCreation
{
    public class CellPrefabCreator
    {
        private readonly IPoolService _poolService;

        private readonly BoardCreationData _creationData;

        private readonly Dictionary<CellType, PrefabFactory> _factories;

        [Inject]
        public CellPrefabCreator(IPoolService poolService, BoardCreationData data)
        {
            _poolService = poolService;
            _creationData = data;

            _factories = new Dictionary<CellType, PrefabFactory>();
        }

        public GameObject Get(CellData cellData)
        {
            var cellType = cellData.cellType;
            _factories.TryGetValue(cellType, out var factory);
            factory = GetFactory(cellData.cellType);
            return factory.Get(cellData);
        }

        public void Return(Cell cell, GameObject cellGameObject)
        {
            var cellType = cell.CellType;
            _factories.TryGetValue(cellType, out var factory);
            factory?.Return(cell, cellGameObject);
        }

        private PrefabFactory GetFactory(CellType cellType)
        {
            var exist = _factories.TryGetValue(cellType, out var factory);
            if (exist)
                return factory;

            factory = cellType switch
            {
                CellType.Block => new BlockPrefabFactory(_poolService, _creationData),
                CellType.Obstacle => new ObstaclePrefabFactory(_poolService, _creationData),
                _ => null
            };

            _factories.Add(cellType, factory);
            return factory;
        }
    }
}