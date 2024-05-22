using System;
using Cysharp.Threading.Tasks;
using GamePlay;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;

namespace PowerUpManagement.PowerUpTypes
{
    public abstract class PowerUp : Cell
    {
        private readonly CellCreator _cellCreator;
        private readonly CellPrefabCreator _cellPrefabCreator;
        public event Action<Cell> OnExplode;
        
        protected PowerUp(CellCreator cellCreator, CellPrefabCreator cellPrefabCreator)
        {
            _cellCreator = cellCreator;
            _cellPrefabCreator = cellPrefabCreator;
        }

        public abstract UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter);

        protected void OnExplodeInvoker(Cell cell) => OnExplode?.Invoke(cell);

        protected void Return(Cell cell)
        {
            cell.Reset();
            _cellCreator.ReturnCell(cell);
            _cellPrefabCreator.Return(cell);
        }

        protected BoardLocation GetObstacleLocation(Cell[,] board, BoardLocation currentLocation)
        {
            var location = currentLocation;
            for (int i = currentLocation.y - 1; i >= 0; i--)
            {
                if(board[currentLocation.x, i] != null)
                    break;

                location.y = i;
            }

            return location;
        }
    }
}