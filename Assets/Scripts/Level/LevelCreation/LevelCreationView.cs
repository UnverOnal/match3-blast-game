using System;
using System.Collections.Generic;
using GamePlay.Board;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
using Level.LevelCounter;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Level.LevelCreation
{
    public class LevelCreationView
    {
        public event Action<CellCreationData> OnPlaceBlock;

        private readonly BoardCreationData _creationData;
        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly BoardResources _boardResources;

        public LevelCreationView(BoardCreationData creationData, CellPrefabCreator cellPrefabCreator,
            BoardResources boardResources)
        {
            _creationData = creationData;
            _cellPrefabCreator = cellPrefabCreator;
            _boardResources = boardResources;
        }

        public void SetBoardBackground(Bounds bounds)
        {
            var background = _boardResources.boardBackground;
            background.sprite = _creationData.background;
            background.gameObject.SetActive(true);

            // Convert bounds to screen space
            var mainCamera = Camera.main;
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
            var cellDatas = GetCellDatas(levelData);

            for (var i = 0; i < levelData.width; i++)
            for (var j = 0; j < levelData.height; j++)
            {
                var cellPosition = new Vector3(i, j, 0);

                var blockGameObject = GetBlock(cellDatas, out var cellData);
                blockGameObject.transform.position = cellPosition;

                OnPlaceBlock?.Invoke(new CellCreationData(new BoardLocation(i, j), blockGameObject, cellData));
            }
        }

        private List<CellData> GetCellDatas(LevelData levelData)
        {
            var cellDatas = new List<CellData>();
            
            cellDatas.AddRange(levelData.blockData);
            cellDatas.AddRange(levelData.obstacleData);

            return cellDatas;
        }

        //Also updates block counts list
        private GameObject GetBlock(IList<CellData> cellDatas, out CellData cellData)
        {
            var randomIndex = Random.Range(0, cellDatas.Count);
            cellData = (CellData)cellDatas[randomIndex].Clone() ;
            cellData.amount--;

            cellDatas[randomIndex] = cellData;
            if (cellData.amount <= 0)
                cellDatas.RemoveAt(randomIndex);

            var blockGameObject = _cellPrefabCreator.Get(cellData);
            blockGameObject.SetActive(true);
            return blockGameObject;
        }
    }
}