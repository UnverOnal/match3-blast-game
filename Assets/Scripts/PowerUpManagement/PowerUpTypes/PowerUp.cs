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
        public CellType type;

        public override void SetData(CellCreationData cellCreationData)
        {
            base.SetData(cellCreationData);
            GameObject.SetActive(true);
            var creationData = (LevelPowerUpData)cellCreationData.levelCellData;
            type = creationData.cellType;
        }

        public abstract UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter,
            CellPrefabCreator cellPrefabCreator);

        protected void OnExplodeInvoker(Cell cell) => OnExplode?.Invoke(cell);
    }
}