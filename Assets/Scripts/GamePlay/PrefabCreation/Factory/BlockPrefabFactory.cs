using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;

namespace GamePlay.PrefabCreation.Factory
{
    public class BlockPrefabFactory : PrefabFactory
    {
        private readonly Dictionary<BlockType ,ObjectPool<GameObject>> _blockPools;
        public BlockPrefabFactory(IPoolService poolService, BoardCreationData creationData) : base(poolService, creationData)
        {
            _blockPools = new Dictionary<BlockType, ObjectPool<GameObject>>();
        }

        public override GameObject Get(LevelCellData levelCellData)
        {
            var blockData = (LevelBlockData)levelCellData;
            var exist = _blockPools.TryGetValue(blockData.type, out var blockPool);
            if (!exist)
            {
                var prefab = GetPrefab(blockData.type);
                blockPool = CreatePool(prefab, blockParent.transform);
                _blockPools.Add(blockData.type, blockPool);
            }
            
            return blockPool.Get();
        }

        private GameObject GetPrefab(BlockType type)
        {
            var blocks = creationData.blockCreationData;
            for (var i = 0; i < blocks.Length; i++)
                if(blocks[i].type == type) return blocks[i].prefab;
    
            return null;
        }
    
        public override void Return(Cell cell, GameObject gameObject)
        {
            var block = (Block)cell;
            _blockPools.TryGetValue(block.BlockType, out var blockPool);
            blockPool?.Return(gameObject);
        }
    }
}
