using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
using Level.LevelCounter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Board
{
    public class BoardView
    {
        public event Action<CellCreationData> OnFillBlock;
        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly LevelPresenter _levelPresenter;
        private readonly BlockMovement _blockMovement;

        public BoardView(CellPrefabCreator cellPrefabCreator, LevelPresenter levelPresenter, BlockMovementData movementData)
        {
            _cellPrefabCreator = cellPrefabCreator;
            _levelPresenter = levelPresenter;

            _blockMovement = new BlockMovement(movementData);
        }

        public async UniTask Shake(Transform transform, float duration, float strength)
        {
            var originalRotation = transform.rotation;
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                var randomAngle = Random.Range(-1f, 1f) * strength;
                var rotationAmount = Quaternion.Euler(0f, 0f, randomAngle);
                transform.rotation = originalRotation * rotationAmount;

                await UniTask.DelayFrame(1);

                elapsedTime += Time.deltaTime;
            }

            transform.rotation = originalRotation;
        }

        public async UniTask Blast(CellGroup cellGroup, GameObject selectedBlockGameObject)
        {
            var center = selectedBlockGameObject.transform.position;
            var cells = cellGroup.cells;

            var tasks = new List<UniTask>();
            foreach (var cellPair in cells)
            {
                var transform = cellPair.Key.transform;
                var originalScale = transform.localScale;

                var tween = _blockMovement.Blast(transform, center);
                tween.OnComplete(() => ReturnToPool(cellPair, originalScale));
                tasks.Add(tween.AsyncWaitForCompletion().AsUniTask());
            }

            await UniTask.WhenAll(tasks);
        }

        public void Collapse(Cell cell)
        {
            var transform = cell.GameObject.transform;

            var targetPosition = transform.position;
            targetPosition.x = cell.Location.x;
            targetPosition.y = cell.Location.y;

            _blockMovement.Fall(transform, targetPosition, true);
        }

        public void Fill(List<List<BoardLocation>> emptyLocations, int boardHeight)
        {
            foreach (var locations in emptyLocations)
                for (var i = 0; i < locations.Count; i++)
                {
                    var location = locations[i];
                    var blockGameObject = GetRandomBlock(out var blockData);
                    var spawnPosition = new Vector3(location.x, boardHeight + i, 0f);
                    blockGameObject.transform.position = spawnPosition;

                    _blockMovement.FallDelayed(blockGameObject.transform, new Vector3(location.x, location.y), true, i);

                    OnFillBlock?.Invoke(new CellCreationData(location, blockGameObject, blockData));
                }
        }

        public void Shuffle(Cell[,] cells)
        {
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                var cell = cells[i, j];
                if (cell == null)
                    continue;

                var location = cell.Location;
                var transform = cell.GameObject.transform;
                transform.DOMove(new Vector3(location.x, location.y), 0.35f).SetEase(Ease.OutBack);
            }
        }

        private void ReturnToPool(KeyValuePair<GameObject, Cell> cellPair, Vector3 originalScale)
        {
            var cell = cellPair.Value;
            var gameObject = cellPair.Key;
            
            gameObject.SetActive(false);
            gameObject.transform.localScale = originalScale;
            _cellPrefabCreator.Return(cell, gameObject);
        }

        private GameObject GetRandomBlock(out CellData cellData)
        {
            var blocks = _levelPresenter.GetCurrentLevelData().blockData;
            cellData = blocks[Random.Range(0, blocks.Length)];
            var blockGameObject = _cellPrefabCreator.Get(cellData);
            blockGameObject.SetActive(true);
            return blockGameObject;
        }
    }
}