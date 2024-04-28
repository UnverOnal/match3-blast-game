using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;

namespace GamePlay.PrefabCreation.Factory
{
    public class ObstaclePrefabFactory : PrefabFactory
    {
        private readonly Dictionary<ObstacleType, ObjectPool<GameObject>> _obstaclePools;

        public ObstaclePrefabFactory(IPoolService poolService, BoardCreationData creationData) : base(poolService, creationData)
        {
            _obstaclePools = new Dictionary<ObstacleType, ObjectPool<GameObject>>();
        }

        public override GameObject Get(LevelCellData levelCellData)
        {
            var obstacleData = (LevelObstacleData)levelCellData;
            var exist = _obstaclePools.TryGetValue(obstacleData.type, out var obstaclePool);
            if (!exist)
            {
                var prefab = GetPrefab(obstacleData.type);
                obstaclePool = CreatePool(prefab, blockParent.transform);
                _obstaclePools.Add(obstacleData.type, obstaclePool);
            }
            
            return obstaclePool.Get();
        }
    
        private GameObject GetPrefab(ObstacleType type)
        {
            var obstacles = creationData.obstacleCreationData;
            for (var i = 0; i < obstacles.Length; i++)
                if(obstacles[i].type == type) return obstacles[i].prefab;

            return null;
        }

        public override void Return(Cell cell, GameObject gameObject)
        {
            var obstacle = (Obstacle)cell;
            _obstaclePools.TryGetValue(obstacle.ObstacleType, out var obstaclePool);
            obstaclePool?.Return(gameObject);
        }
    }
}
