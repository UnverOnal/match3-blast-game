using System;
using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace GamePlay
{
    public class CellPrefabCreator
    {
        private readonly IPoolService _poolService;

        private readonly BoardCreationData _creationData;

        private readonly Dictionary<CellType, ObjectPool<GameObject>> _pools;

        private readonly GameObject _prefabParent;

        [Inject]
        public CellPrefabCreator(IPoolService poolService, BoardCreationData data)
        {
            _poolService = poolService;
            _creationData = data;

            _pools = new Dictionary<CellType, ObjectPool<GameObject>>();

            _prefabParent = new GameObject("Cells");
        }

        public GameObject Get(LevelCellData levelCellData)
        {
            var cellType = levelCellData.cellType;
            var poolExist = _pools.TryGetValue(cellType, out var pool);
            if (!poolExist)
            {
                pool = _poolService.GetPoolFactory().CreatePool(CreateCell(cellType));
                _pools.Add(cellType, pool);
            }

            return pool.Get();
        }

        public void Return(Cell cell)
        {
            var cellType = cell.CellType;
            var pool = _pools[cellType];
            pool.Return(cell.GameObject);
        }

        private Func<GameObject> CreateCell(CellType cellType)
        {
            GameObject prefab = null;
            var cellAssetDatas = _creationData.blockCreationData;
            for (int i = 0; i < cellAssetDatas.Length; i++)
            {
                var data = cellAssetDatas[i];
                if (data.type == cellType)
                {
                    prefab = data.prefab;
                    break;
                }
            }

            return () => Object.Instantiate(prefab, parent:_prefabParent.transform);
        }
    }
}