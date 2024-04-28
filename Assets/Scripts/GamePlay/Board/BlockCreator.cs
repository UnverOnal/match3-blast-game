using System.Collections.Generic;
using Level.LevelCounter;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace GamePlay.Board
{
    public class BlockCreator
    {
        private readonly IPoolService _poolService;
        private readonly Dictionary<CellType ,ObjectPool<GameObject>> _blockPools;

        private readonly BoardCreationData _creationData;

        private readonly GameObject _blockParent;

        [Inject]
        public BlockCreator(IPoolService poolService, BoardCreationData data)
        {
            _poolService = poolService;
            _creationData = data;
                
            _blockParent = new GameObject("BlockParent");

            _blockPools = new Dictionary<CellType, ObjectPool<GameObject>>();
        }

        public GameObject GetBlock(CellType type)
        {
            var exist = _blockPools.TryGetValue(type, out var blockPool);
            if (!exist)
            {
                var prefab = GetPrefab(type);
                blockPool = CreatePool(prefab, _blockParent.transform);
                _blockPools.Add(type, blockPool);
            }
            
            return blockPool.Get();
        }

        public void ReturnBlock(CellType type, GameObject gameObject)
        {
            _blockPools.TryGetValue(type, out var blockPool);
            blockPool?.Return(gameObject);
        }

        private ObjectPool<GameObject> CreatePool(GameObject prefab, Transform parent)
        {
            var pool = _poolService.GetPoolFactory().CreatePool(() => Object.Instantiate(prefab, parent: parent));
            return pool;
        }

        private GameObject GetPrefab(CellType type)
        {
            var blocks = _creationData.blockData;
            for (var i = 0; i < blocks.Length; i++)
                if(blocks[i].type == type) return blocks[i].prefab;

            return null;
        }
    }
}