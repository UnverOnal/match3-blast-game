using GamePlay.CellManagement;
using Level.LevelCreation;
using Services.PoolingService;
using UnityEngine;

namespace GamePlay.PrefabCreation.Factory
{
    public abstract class PrefabFactory
    {
        private readonly IPoolService _poolService;
        protected readonly BoardCreationData creationData;
        protected readonly GameObject blockParent;

        protected PrefabFactory(IPoolService poolService, BoardCreationData creationData)
        {
            this._poolService = poolService;
            this.creationData = creationData;
            blockParent = new GameObject(GetType().ToString());
        }

        public abstract GameObject Get(CellData cellData);

        public abstract void Return(Cell cell, GameObject gameObject);
    
        protected ObjectPool<GameObject> CreatePool(GameObject prefab, Transform parent)
        {
            var pool = _poolService.GetPoolFactory().CreatePool(() => Object.Instantiate(prefab, parent: parent));
            return pool;
        }
    }
}
