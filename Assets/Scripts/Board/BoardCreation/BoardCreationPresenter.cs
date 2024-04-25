using System.Collections.Generic;
using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Board.BoardCreation
{
    public class BoardCreationPresenter
    {
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;
        
        private readonly BoardCreationData _creationData;
        private readonly BoardCreationView _boardCreationView;
        private readonly BlockCreator _blockCreator;
        
        public BoardCreationPresenter(IPoolService poolService, BoardCreationData creationData)
        {
            _boardCreationView = new BoardCreationView();
            _creationData = creationData;
            
            _blockCreator = new BlockCreator(poolService, creationData);
        }

        public void Create()
        {
            var levelData = _levelPresenter.GetNextLevelData();
            GenerateGrid(levelData);
            //Set camera
        }
        
        private void GenerateGrid(LevelData levelData)
        {
            var blockCounts = GetBlockCounts(levelData);
            
            var startPosition = new Vector3(_creationData.centerPoint.x - (levelData.gridSize.rows - 1) / 2f,
                _creationData.centerPoint.y - (levelData.gridSize.columns - 1) / 2f, 0);

            for (var row = 0; row < levelData.gridSize.rows; row++)
            for (var column = 0; column < levelData.gridSize.columns; column++)
            {
                var cellPosition = new Vector3(startPosition.x + row, startPosition.y + column, 0);

                var block = GetBlock(blockCounts);
                block.transform.position = cellPosition;
            }
        }

        private List<KeyValuePair<BlockType, int>> GetBlockCounts(LevelData levelData)
        {
            var blockCounts = new List<KeyValuePair<BlockType, int>>();
            var obstacleCount = new KeyValuePair<BlockType, int>(BlockType.Obstacle, levelData.obstacleHealths.Length);
            blockCounts.Add(obstacleCount);
            for (int i = 0; i < levelData.blockData.Length; i++)
            {
                var blockData = levelData.blockData[i];
                var blockCount = new KeyValuePair<BlockType, int>(blockData.type, blockData.amount);
                blockCounts.Add(blockCount);
            }

            return blockCounts;
        }

        //Also updates block counts list
        private GameObject GetBlock(IList<KeyValuePair<BlockType, int>> blockCounts)
        {
            var randomIndex = Random.Range(0, blockCounts.Count);
            var (type, count) = blockCounts[randomIndex];
            count--;
            
            blockCounts[randomIndex] = new KeyValuePair<BlockType, int>(type, count);
            if (count <= 0)
                blockCounts.RemoveAt(randomIndex);

            return _blockCreator.GetBlock(type);
        }
    }
}
