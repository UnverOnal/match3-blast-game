using System;
using System.Collections.Generic;
using GamePlay.Board;
using GamePlay.CellManagement;
using Level.LevelCounter;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Level.LevelCreation
{
    public class LevelCreationView
    {
        public event Action<CellData> OnPlaceBlock;

        private readonly BoardCreationData _creationData;
        private readonly BlockCreator _blockCreator;
        private readonly BoardResources _boardResources;

        public LevelCreationView(BoardCreationData creationData, BlockCreator blockCreator, BoardResources boardResources)
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

            for (var i = 0; i < levelData.boardSize.x; i++)
            for (var j = 0; j < levelData.boardSize.y; j++)
            {
                var cellPosition = new Vector3(i, j, 0);

                var block = GetBlock(blockCounts, out var type);
                block.transform.position = cellPosition;
                
                OnPlaceBlock?.Invoke(new CellData(new BoardLocation(i,j), block, type));
            }
        }

        private List<KeyValuePair<CellType, int>> GetBlockCounts(LevelData levelData)
        {
            var blockCounts = new List<KeyValuePair<CellType, int>>();
            for (int i = 0; i < levelData.blockData.Length; i++)
            {
                var blockData = levelData.blockData[i];
                var blockCount = new KeyValuePair<CellType, int>(blockData.type, blockData.amount);
                blockCounts.Add(blockCount);
            }
            var obstacleCount = new KeyValuePair<CellType, int>(CellType.Obstacle, levelData.obstacleHealths.Length);
            blockCounts.Add( obstacleCount);
            
            return blockCounts;
        }
        
        //Also updates block counts list
        private GameObject GetBlock(IList<KeyValuePair<CellType, int>> blockCounts, out CellType cellType)
        {
            var randomIndex = Random.Range(0, blockCounts.Count);
            var (type, count) = blockCounts[randomIndex];
            count--;
            
            blockCounts[randomIndex] = new KeyValuePair<CellType, int>(type, count);
            if (count <= 0)
                blockCounts.RemoveAt(randomIndex);

            cellType = type;
            var block = _blockCreator.GetBlock(type);
            block.SetActive(true);
            return block;
        }
    }
}