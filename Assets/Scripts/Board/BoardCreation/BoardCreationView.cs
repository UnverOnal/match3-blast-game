using System;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Board.BoardCreation
{
    public class BoardCreationView
    {
        public event Action<BoardLocation, GameObject> OnBlockCreated;

        private readonly BoardCreationData _creationData;
        private readonly BlockCreator _blockCreator;

        public BoardCreationView(BoardCreationData creationData, BlockCreator blockCreator)
        {
            _creationData = creationData;
            _blockCreator = blockCreator;
        }

        public void PlaceBlocks(LevelData levelData)
        {
            var blockCounts = GetBlockCounts(levelData);
            
            var startPosition = new Vector3(_creationData.centerPoint.x - (levelData.boardSize.rows - 1) / 2f,
                _creationData.centerPoint.y - (levelData.boardSize.columns - 1) / 2f, 0);

            for (var row = 0; row < levelData.boardSize.rows; row++)
            for (var column = 0; column < levelData.boardSize.columns; column++)
            {
                var cellPosition = new Vector3(startPosition.x + row, startPosition.y + column, 0);

                var block = GetBlock(blockCounts);
                block.transform.position = cellPosition;
                
                OnBlockCreated?.Invoke(new BoardLocation(row, column), block);
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