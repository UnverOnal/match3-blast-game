using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PowerUpManagement.PowerUpTypes
{
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public class Rocket : PowerUp
    {
        private const float Speed = 10f;

        private bool canDo;

        public override async UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter,
            CellPrefabCreator cellPrefabCreator)
        {
            var tasks = new List<UniTask>();

            spriteRenderer.enabled = false;
            var transform = GameObject.transform;

            tasks.Add(Fire(transform.GetChild(0), Vector3.right * -1f, fillPresenter, board, cellPrefabCreator));
            tasks.Add(Fire(transform.GetChild(1), Vector3.right, fillPresenter, board, cellPrefabCreator));

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Fire(Transform transform, Vector3 direction, BoardFillPresenter fillPresenter,
            Cell[,] board, CellPrefabCreator cellPrefabCreator)
        {
            transform.gameObject.SetActive(true);

            var cells = GetCells(direction, board);
            var targetPosition = GetTargetPosition(direction, transform);
            var duration = Vector3.Distance(transform.position, targetPosition) / Speed;
            var tween = transform.DOMove(targetPosition, duration).OnUpdate(() =>
            {
                if(cells.Count == 0) return;
                var nextCell = cells[0];
                var nextCellPosition = nextCell.GameObject.transform.position;
                if (!(Vector3.Distance(nextCellPosition, transform.position) <
                      0.3f)) return;
                
                ExplodeCell(board, fillPresenter, nextCell, cellPrefabCreator);
                cells.RemoveAt(0);
            });

            await tween.AsyncWaitForCompletion().AsUniTask();
        }

        private Vector3 GetTargetPosition(Vector3 direction, Transform transform)
        {
            var camera = Camera.main;
            
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            
            var targetPosition = camera.WorldToScreenPoint(transform.position);
            targetPosition.z = 10f;

            if (direction == Vector3.right)
                targetPosition.x = screenWidth;
            if (direction == Vector3.left)
                targetPosition.x = 0f;
            if (direction == Vector3.up)
                targetPosition.y = screenHeight;
            if(direction == Vector3.down)
                targetPosition.y = 0f;

            targetPosition = camera.ScreenToWorldPoint(targetPosition + direction * 100f);
            targetPosition.z = 0f;
            return targetPosition;
        }

        private async void ExplodeCell(Cell[,] board, BoardFillPresenter fillPresenter, Cell nextCell,
            CellPrefabCreator cellPrefabCreator)
        {
            if (nextCell == null) return;

            var cellLocation = nextCell.Location;
            var cell = board[cellLocation.x, cellLocation.y];

            if (cell != this)
                cell.Destroy();

            OnExplodeInvoker(cell);

            fillPresenter.CollapseColumn(cellLocation, board);
            await fillPresenter.FillColumn(cellLocation, board);
        }

        private List<Cell> GetCells(Vector3 direction, Cell[,] board)
        {
            var cells = new List<Cell>();
            var currentX = Location.x;
            var currentY = Location.y;
            var width = board.GetLength(0);
            var height = board.GetLength(1);

            if (direction == Vector3.left)
                while (currentX >= 0)
                {
                    if (currentX < width && currentY >= 0 && currentY < height) cells.Add(board[currentX, currentY]);
                    currentX--;
                }
            else if (direction == Vector3.right)
            {
                currentX++;
                while (currentX < width)
                {
                    if (currentX >= 0 && currentY >= 0 && currentY < height) cells.Add(board[currentX, currentY]);
                    currentX++;
                }
            }

            return cells;
        }

        public override void Reset()
        {
            base.Reset();

            spriteRenderer.enabled = true;

            var transform = GameObject.transform;

            transform.GetChild(0).position = Position;
            transform.GetChild(1).position = Position;

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}