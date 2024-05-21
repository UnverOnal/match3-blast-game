using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;

namespace PowerUpManagement.PowerUpTypes
{
    public class Bomb : PowerUp
    {
        private readonly Dictionary<int, BoardLocation> _bottomLocations = new();

        public Bomb(CellCreator cellCreator, CellPrefabCreator cellPrefabCreator) : base(cellCreator, cellPrefabCreator)
        {
        }

        public override async UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter)
        {
            var tasks = new List<UniTask>();
            var cellsToExplode = GetCells(board);

            for (var i = 0; i < cellsToExplode.Count; i++)
            {
                var cell = cellsToExplode[i];
                if (cell == null)
                    continue;

                var task = cell.Destroy();
                task.OnComplete(() => Return(cell));
                OnExplodeInvoker(cell);
                tasks.Add(task.AsyncWaitForCompletion().AsUniTask());
            }

            await UniTask.WhenAll(tasks);

            Fill(_bottomLocations, board, fillPresenter);
        }

        private void Fill(Dictionary<int, BoardLocation> bottomLocations, Cell[,] board,
            BoardFillPresenter fillPresenter)
        {
            foreach (var locationPair in bottomLocations)
            {
                var location = locationPair.Value;
                fillPresenter.CollapseColumn(location, board);
                fillPresenter.FillColumn(location, board);
            }
        }

        private List<Cell> GetCells(Cell[,] board)
        {
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            var bombLocation = Location;

            var neighbours = new List<Cell>();

            foreach (var dx in new List<int> { -1, 0, 1 })
            foreach (var dy in new List<int> { -1, 0, 1 })
            {
                if (dx == 0 && dy == 0) continue;

                var neighbourX = bombLocation.x + dx;
                var neighbourY = bombLocation.y + dy;

                if (!IsValidCell(neighbourX, neighbourY, width, height)) continue;
                var neighbour = board[neighbourX, neighbourY];
                neighbours.Add(neighbour);
                SetBottomLocation(neighbour);
            }

            neighbours.Add(this);
            SetBottomLocation(this);
            return neighbours;
        }

        private bool IsValidCell(int x, int y, int width, int height)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private void SetBottomLocation(Cell neighbour)
        {
            if (neighbour == null) return;

            var neighbourLocation = neighbour.Location;
            var locationExist = _bottomLocations.TryGetValue(neighbourLocation.x, out var location);
            switch (locationExist)
            {
                case true when neighbourLocation.y < location.y:
                    _bottomLocations[neighbourLocation.x] = neighbourLocation;
                    break;
                case false:
                    _bottomLocations.Add(neighbourLocation.x, neighbourLocation);
                    break;
            }
        }
    }
}