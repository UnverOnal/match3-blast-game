using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using UnityEngine;

namespace PowerUpManagement.PowerUpTypes
{
    public class Rocket : PowerUp
    {
        private const float Speed = 10f;

        public override async UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter)
        {
            var tasks = new List<UniTask>();
            
            spriteRenderer.enabled = false;
            var transform = GameObject.transform;

            tasks.Add(Fire(transform.GetChild(0), Vector3.left, fillPresenter, board));
            tasks.Add(Fire(transform.GetChild(1), Vector3.right, fillPresenter, board));

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Fire(Transform transform, Vector3 direction, BoardFillPresenter fillPresenter, Cell[,] board)
        {
            transform.gameObject.SetActive(true);

            var cells = GetCells(direction, board);
            var targetPosition = cells[^1].Position + direction * 1.25f;
            var duration = Vector3.Distance(transform.position, targetPosition) / Speed;
            var tween = transform.DOMove(targetPosition, duration).OnUpdate(() =>
            {
                var nextCell = cells[0];
                var nextCellPosition = nextCell.GameObject.transform.position;
                if (!(Vector3.Distance(nextCellPosition, transform.position) <
                      0.3f)) return;
                ExplodeCell(board, fillPresenter, nextCell);
                cells.RemoveAt(0);
            });

            await tween.AsyncWaitForCompletion().AsUniTask();
        }

        private void ExplodeCell(Cell[,] board, BoardFillPresenter fillPresenter, Cell nextCell)
        {
            if (nextCell == null) return;

            var cellLocation = nextCell.Location;
            var cell = board[cellLocation.x, cellLocation.y];

            if (cell != this)
                cell.Destroy().OnComplete(()=> cell.Reset());

            OnExplodeInvoker(cell);

            fillPresenter.CollapseColumn(cellLocation, board);
            fillPresenter.FillColumn(cellLocation, board);
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
                while (currentX < width)
                {
                    if (currentX >= 0 && currentY >= 0 && currentY < height) cells.Add(board[currentX, currentY]);
                    currentX++;
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