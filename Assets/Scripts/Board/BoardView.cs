using System;
using System.Collections.Generic;
using Board.BoardCreation;
using Board.CellManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Board
{
    public class BoardView
    {
        public event Action<CellData> OnBlockCreated;
        private readonly BlockCreator _blockCreator;
        private readonly LevelPresenter _levelPresenter;

        public BoardView(BlockCreator blockCreator, LevelPresenter levelPresenter)
        {
            _blockCreator = blockCreator;
            _levelPresenter = levelPresenter;
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
                var tween = transform.DOMove(center, 0.25f).SetEase(Ease.InBack)
                    .OnComplete(() => transform.DOScale(0f, 0.1f).SetEase(Ease.InBack, 2f));
                tasks.Add(tween.AsyncWaitForCompletion().AsUniTask());
            }

            await UniTask.WhenAll(tasks);
            selectedBlock.transform.DOScale(0f, 0.1f).SetEase(Ease.InBack, 2f);
        }

        public async void Collapse(Cell cell, int delayAmount)
        {
            // await UniTask.Delay(TimeSpan.FromSeconds(0.05f * delayAmount));
            
            var transform = cell.GameObject.transform;
            
            var targetPosition = transform.position;
            targetPosition.x = cell.Location.x;
            targetPosition.y = cell.Location.y;

            transform.DOMove(targetPosition, 0.25f).SetEase(Ease.OutBack);
        }

        public void Fill(List<BoardLocation> emptyLocations, int boardHeight)
        {
            for (int i = 0; i < emptyLocations.Count; i++)
            {
                var location = emptyLocations[i];
                var block = GetRandomBlock(out var blockType);
                var spawnPosition = new Vector3(location.x, boardHeight + i, 0f);
                block.transform.position = spawnPosition;
                
                block.transform.DOMove(new Vector3(location.x, location.y), 0.25f).SetEase(Ease.OutBack);
                
                OnBlockCreated?.Invoke(new CellData(location, block, blockType));
            }
        }

        private GameObject GetRandomBlock(out BlockType type)
        {
            var blocks = _levelPresenter.GetCurrentLevelData().blockData;
            type = blocks[Random.Range(0, blocks.Length)].type;
            return _blockCreator.GetBlock(type);
        }
        
        public void Shuffle()
        {
        }
    }
}