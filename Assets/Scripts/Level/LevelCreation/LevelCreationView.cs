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
            var blockDatas = GetBlockCounts(levelData);

            for (var i = 0; i < levelData.boardSize.x; i++)
            for (var j = 0; j < levelData.boardSize.y; j++)
            {
                var cellPosition = new Vector3(i, j, 0);

                var blockGameObject = GetBlock(blockDatas, out var blockData);
                blockGameObject.transform.position = cellPosition;
                
                OnPlaceBlock?.Invoke(new CellData(new BoardLocation(i,j), blockGameObject, blockData));
            }
        }

        private List<LevelData.BlockData> GetBlockCounts(LevelData levelData)
        {
            var blockDatas = new List<LevelData.BlockData>();
            for (int i = 0; i < levelData.blockData.Length; i++)
                blockDatas.Add(levelData.blockData[i]);
            
            var obstacleData = new LevelData.BlockData()
                { amount = levelData.obstacleHealths.Length, type = CellType.Obstacle };
            blockDatas.Add( obstacleData);
            
            return blockDatas;
        }
        
        //Also updates block counts list
        private GameObject GetBlock(IList<LevelData.BlockData> blockDatas, out LevelData.BlockData blockData)
        {
            var randomIndex = Random.Range(0, blockDatas.Count);
            blockData = blockDatas[randomIndex];
            blockData.amount--;

            blockDatas[randomIndex] = blockData;
            if (blockData.amount <= 0)
                blockDatas.RemoveAt(randomIndex);

            var blockGameObject = _blockCreator.GetBlock(blockData.type);
            blockGameObject.SetActive(true);
            return blockGameObject;
        }
    }
}