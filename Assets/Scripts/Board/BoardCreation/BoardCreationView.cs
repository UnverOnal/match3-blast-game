using System;
using System.Collections.Generic;
using Board.CellManagement;
using Level;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Board.BoardCreation
{
    public class BoardCreationView
    {
        public event Action<CellData> OnBlockCreated;

        private readonly BoardCreationData _creationData;
        private readonly BlockCreator _blockCreator;
        private readonly BoardResources _boardResources;

        public BoardCreationView(BoardCreationData creationData, BlockCreator blockCreator, BoardResources boardResources)
        {
            _creationData = creationData;
            _blockCreator = blockCreator;
            _boardResources = boardResources;
        }

        public void SetBoardBackground(Bounds bounds)
        {
            var background = _boardResources.boardBackground;
            background.sprite = _creationData.background;
            background.gameObject.SetActive(true);
            
            // Convert bounds to screen space
            Camera mainCamera = Camera.main;
            var minScreenPoint = mainCamera.WorldToScreenPoint(bounds.min);
            var maxScreenPoint = mainCamera.WorldToScreenPoint(bounds.max);
            // Calculate occupied area
            var occupiedWidth = maxScreenPoint.x - minScreenPoint.x;
            var occupiedHeight = maxScreenPoint.y - minScreenPoint.y;
            
            background.rectTransform.sizeDelta = new Vector2(occupiedWidth, occupiedHeight);
            background.type = Image.Type.Sliced;
            background.GetComponentInParent<Canvas>().worldCamera = mainCamera;
        }

        public void PlaceBlocks(LevelData levelData)
        {
            var blockCounts = GetBlockCounts(levelData);
            
            var startPosition = new Vector3(_creationData.centerPoint.x - (levelData.boardSize.x - 1) / 2f,
                _creationData.centerPoint.y - (levelData.boardSize.y - 1) / 2f, 0);

            for (var i = 0; i < levelData.boardSize.x; i++)
            for (var j = 0; j < levelData.boardSize.y; j++)
            {
                var cellPosition = new Vector3(startPosition.x + i, startPosition.y + j, 0);

                var block = GetBlock(blockCounts, out var type);
                block.transform.position = cellPosition;
                
                OnBlockCreated?.Invoke(new CellData(new BoardLocation(i,j), block, type));
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
        private GameObject GetBlock(IList<KeyValuePair<BlockType, int>> blockCounts, out BlockType blockType)
        {
            var randomIndex = Random.Range(0, blockCounts.Count);
            var (type, count) = blockCounts[randomIndex];
            count--;
            
            blockCounts[randomIndex] = new KeyValuePair<BlockType, int>(type, count);
            if (count <= 0)
                blockCounts.RemoveAt(randomIndex);

            blockType = type;
            return _blockCreator.GetBlock(type);
        }
    }
}