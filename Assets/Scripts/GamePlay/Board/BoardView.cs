using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using GamePlay.CellManagement;
using Level.LevelCounter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Board
{
    public class BoardView
    {
        public event Action<CellData> OnFillBlock;
        private readonly BlockCreator _blockCreator;
        private readonly LevelPresenter _levelPresenter;
        private readonly BlockFall _blockFall;
        private readonly BlockMovementData _movementData;

        public BoardView(BlockCreator blockCreator, LevelPresenter levelPresenter, GameSettings gameSettings)
        {
            _blockCreator = blockCreator;
            _levelPresenter = levelPresenter;
            _movementData = gameSettings.blockMovementData;

            _blockFall = new BlockFall(_movementData);
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

        public async UniTask Blast(CellGroup cellGroup, GameObject selectedBlock)
        {
            var center = selectedBlock.transform.position;
            var cells = cellGroup.cells;

            var tasks = new List<UniTask>();
            foreach (var cellPair in cells)
            {
                var transform = cellPair.Key.transform;
                var originalScale = transform.localScale;
                
                var tween = DOTween.Sequence();
                tween.Append(transform.DOMove(center, _movementData.blastDuration).SetEase(Ease.InBack));
                tween.Append(transform.DOScale(0f, 0.1f).SetEase(Ease.InBack, 2f));
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
            
            _blockFall.Fall(transform, targetPosition, true);
        }

        public void Fill(List<List<BoardLocation>> emptyLocations, int boardHeight)
        {
            foreach (var locations in emptyLocations)
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    var location = locations[i];
                    var block = GetRandomBlock(out var blockType);
                    var spawnPosition = new Vector3(location.x, boardHeight + i, 0f);
                    block.transform.position = spawnPosition;

                    _blockFall.FallDelayed(block.transform, new Vector3(location.x, location.y), true, i);

                    OnFillBlock?.Invoke(new CellData(location, block, blockType));
                }
            }
        }

        public void Shuffle(Cell[,] cells)
        {
            var rows = cells.GetLength(0);
            var cols = cells.GetLength(1);
            
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var cell = cells[i, j];
                    if (cell == null)
                        continue;
                    
                    var location = cell.Location;
                    var transform = cell.GameObject.transform;
                    transform.DOMove(new Vector3(location.x, location.y), 0.35f).SetEase(Ease.OutBack);
                }
            }
        }
        
        private void ReturnToPool(KeyValuePair<GameObject, Cell> cellPair, Vector3 originalScale)
        {
            cellPair.Key.SetActive(false);
            cellPair.Key.transform.localScale = originalScale;
            _blockCreator.ReturnBlock(cellPair.Value.CellType, cellPair.Key);
        }
        
        private GameObject GetRandomBlock(out CellType type)
        {
            var blocks = _levelPresenter.GetCurrentLevelData().blockData;
            type = blocks[Random.Range(0, blocks.Length)].type;
            var block = _blockCreator.GetBlock(type);
            block.SetActive(true);
            return block;
        }
    }
}