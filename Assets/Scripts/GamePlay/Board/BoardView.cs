using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using GamePlay.CellManagement;
using GamePlay.PrefabCreation;
using Level.LevelCounter;
using Unity.VisualScripting;
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
            var cells = cellGroup.blocks;

            var tasks = new List<UniTask>();
            foreach (var cell in cells)
            {
                var transform = cell.GameObject.transform;
                var originalScale = transform.localScale;

                var tween = _blockMovement.Blast(transform, center);
                tween.OnComplete(() => ReturnToPool(cell, originalScale));
                tasks.Add(tween.AsyncWaitForCompletion().AsUniTask());
            }

            await UniTask.WhenAll(tasks);
        }

        public void ExplodeDamageables(CellGroup cellGroup)
        {
            var explodeables = cellGroup.explodeables;
            foreach (var explodeable in explodeables)
                explodeable.Explode();
        }

        public void Collapse(Cell cell)
        {
            var transform = cell.GameObject.transform;

            var targetPosition = transform.position;
            targetPosition.x = cell.Location.x;
            targetPosition.y = cell.Location.y;

            _blockMovement.Fall(transform, targetPosition, true);
        }

        public async UniTask Fill(List<List<BoardLocation>> emptyLocations, int boardHeight)
        {
            var tasks = new List<UniTask>();
            foreach (var locations in emptyLocations)
                for (var i = 0; i < locations.Count; i++)
                {
                    var location = locations[i];
                    var blockGameObject = GetRandomBlock(out var blockData);
                    var spawnPosition = new Vector3(location.x, boardHeight + i, 0f);
                    blockGameObject.transform.position = spawnPosition;

                    var task = _blockMovement.FallDelayed(blockGameObject.transform, new Vector3(location.x, location.y), true, i);
                    tasks.Add(task);

                    OnFillBlock?.Invoke(new CellCreationData(location, blockGameObject, blockData));
                }

            await UniTask.WhenAll(tasks);
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

        private void ReturnToPool(Cell cell, Vector3 originalScale)
        {
            var gameObject = cell.GameObject;
            
            gameObject.SetActive(false);
            gameObject.transform.localScale = originalScale;
            _cellPrefabCreator.Return(cell, gameObject);
        }

        private GameObject GetRandomBlock(out LevelCellData levelCellData)
        {
            var blocks = _levelPresenter.GetCurrentLevelData().blockData;
            levelCellData = blocks[Random.Range(0, blocks.Length)];
            var blockGameObject = _cellPrefabCreator.Get(levelCellData);
            blockGameObject.SetActive(true);
            return blockGameObject;
        }
    }
}