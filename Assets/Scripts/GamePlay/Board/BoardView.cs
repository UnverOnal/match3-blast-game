using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameManagement;
using GamePlay.CellManagement;
using UnityEngine;

namespace GamePlay.Board
{
    public class BoardView
    {
        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly BlockMovement _blockMovement;

        public BoardView(CellPrefabCreator cellPrefabCreator, BlockMovementData movementData)
        {
            _cellPrefabCreator = cellPrefabCreator;

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
            var explodeables = cellGroup.explodeableObstacles;
            foreach (var explodeable in explodeables)
                explodeable.Explode();
        }

        public async UniTask Shuffle(Cell[,] cells)
        {
            var tasks = new List<UniTask>();

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
                var tween = transform.DOMove(new Vector3(location.x, location.y), 0.35f).SetEase(Ease.OutBack);
                tasks.Add(tween.AsyncWaitForCompletion().AsUniTask());
            }

            await UniTask.WhenAll(tasks);
        }

        private void ReturnToPool(Cell cell, Vector3 originalScale)
        {
            var gameObject = cell.GameObject;

            gameObject.SetActive(false);
            gameObject.transform.localScale = originalScale;
            _cellPrefabCreator.Return(cell);
        }
    }
}