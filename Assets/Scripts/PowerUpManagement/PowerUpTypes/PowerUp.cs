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
        public event Action<Cell> OnExplode;

        public abstract UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter,
            CellPrefabCreator cellPrefabCreator, CellCreator cellCreator);

        protected void OnExplodeInvoker(Cell cell) => OnExplode?.Invoke(cell);
    }
}