using System;
using System.Collections.Generic;
using GamePlay;
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

        public void CreateBoard(LevelData levelData)
        {
            PlaceObstacles(levelData, out var occupiedLocations);
            PlaceBlocks(levelData, occupiedLocations);
        }

        private void PlaceObstacles(LevelData levelData, out HashSet<BoardLocation> occupiedLocations)
        {
            occupiedLocations = new HashSet<BoardLocation>();

            var obstacleData = levelData.obstacleData;

            for (var i = 0; i < obstacleData.Length; i++)
            {
                var data = obstacleData[i];
                var location = data.location;
                var position = new Vector3(location.x, location.y, 0);

                var obstacleGameObject = _cellPrefabCreator.Get(data.cellType);
                obstacleGameObject.transform.position = position;

                occupiedLocations.Add(location);

                OnPlaceBlock?.Invoke(new CellCreationData(location, obstacleGameObject, data));
            }
        }

        private void PlaceBlocks(LevelData levelData, HashSet<BoardLocation> occupiedLocations)
        {
            var blockDatas = new List<LevelBlockData>(levelData.blockData);

            for (var i = 0; i < levelData.width; i++)
            for (var j = 0; j < levelData.height; j++)
            {
                var location = new BoardLocation(i, j);

                if (occupiedLocations.Contains(location)) continue;

                var cellPosition = new Vector3(location.x, location.y, 0);

                var blockGameObject = GetBlock(blockDatas, out var blockData);
                blockGameObject.transform.position = cellPosition;

                OnPlaceBlock?.Invoke(new CellCreationData(new BoardLocation(i, j), blockGameObject, blockData));
            }
        }

        public void ResetCell(Cell cell)
        {
            if (cell.CellType is CellType.Rocket or CellType.Bomb) return;

            cell.GameObject.SetActive(false);
            _cellPrefabCreator.Return(cell);
        }

        //Also updates block counts list
        private GameObject GetBlock(IList<LevelBlockData> cellDatas, out LevelBlockData levelCellData)
        {
            var randomIndex = Random.Range(0, cellDatas.Count);
            levelCellData = (LevelBlockData)cellDatas[randomIndex].Clone();
            levelCellData.amount--;

            cellDatas[randomIndex] = levelCellData;
            if (levelCellData.amount <= 0)
                cellDatas.RemoveAt(randomIndex);

            var blockGameObject = _cellPrefabCreator.Get(levelCellData.cellType);
            return blockGameObject;
        }
    }
}