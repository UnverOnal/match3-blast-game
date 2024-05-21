using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameManagement;
using GamePlay.CellManagement;
using Level.LevelCounter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Board.Steps.Fill
{
    public class BoardFillView
    {
        public event Action<CellCreationData> OnFillBlock;

        private readonly BlockMovement _blockMovement;
        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly LevelPresenter _levelPresenter;

        public BoardFillView(BlockMovementData movementData, CellPrefabCreator cellPrefabCreator,
            LevelPresenter levelPresenter)
        {
            _cellPrefabCreator = cellPrefabCreator;
            _levelPresenter = levelPresenter;
            _blockMovement = new BlockMovement(movementData);
        }

        public void CollapseColumn(List<Cell> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                var transform = cell.GameObject.transform;

                var targetPosition = transform.position;
                targetPosition.x = cell.Location.x;
                targetPosition.y = cell.Location.y;

                if (targetPosition != transform.position)
                    _blockMovement.Fall(transform, targetPosition, true);
            }
        }

        public async UniTask FillColumn(List<BoardLocation> emptyLocations, int boardHeight)
        {
            var tasks = new List<UniTask>();
            for (var i = 0; i < emptyLocations.Count; i++)
            {
                var location = emptyLocations[i];
                var blockGameObject = GetRandomBlock(out var blockData);
                var spawnPosition = new Vector3(location.x, boardHeight + i, 0f);
                blockGameObject.transform.position = spawnPosition;

                var task = _blockMovement.FallDelayed(blockGameObject.transform, new Vector3(location.x, location.y),
                    true, i);
                tasks.Add(task);

                OnFillBlock?.Invoke(new CellCreationData(location, blockGameObject, blockData));
            }

            await UniTask.WhenAll(tasks);
        }

        private GameObject GetRandomBlock(out LevelCellData levelCellData)
        {
            var blocks = _levelPresenter.GetCurrentLevelData().blockData;
            levelCellData = blocks[Random.Range(0, blocks.Length)];
            var blockGameObject = _cellPrefabCreator.Get(levelCellData.cellType);
            Debug.Log(blockGameObject.activeSelf);
            return blockGameObject;
        }
    }
}