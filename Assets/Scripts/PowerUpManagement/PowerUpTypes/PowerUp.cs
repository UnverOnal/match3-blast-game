using System;
using Cysharp.Threading.Tasks;
using GamePlay;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;

namespace PowerUpManagement.PowerUpTypes
{
    public abstract class PowerUp : Cell
    {
        public event Action<Cell> OnExplode;

        public abstract UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter,
            CellPrefabCreator cellPrefabCreator);

        protected void OnExplodeInvoker(Cell cell) => OnExplode?.Invoke(cell);
    }
}