using System;
using Cysharp.Threading.Tasks;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;

namespace PowerUpManagement.PowerUpTypes
{
    public abstract class PowerUp : Cell
    {
        public event Action<Cell> OnExplode;
        public PowerUpType type;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            var creationData = (LevelPowerUpData)cellCreationData.levelCellData;
            type = creationData.type;
        }

        public override void Reset()
        {
            Location = new BoardLocation();
        }

        public abstract UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter);

        protected void OnExplodeInvoker(Cell cell)
        {
            OnExplode?.Invoke(cell);
        }
    }
}
